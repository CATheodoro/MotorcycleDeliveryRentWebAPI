using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using System.Security.Claims;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMotorcycleService _motorcycleService;
        private readonly IDriverService _driverService;
        private readonly IPlanService _planService;
        private readonly ILogger<RentService> _logger;

        public RentService(IRentRepository rentRepository, IHttpContextAccessor httpContextAccessor,
            IMotorcycleService motorcycleService, IDriverService driverService, IPlanService planService, ILogger<RentService> logger)
        {
            _repository = rentRepository;
            _httpContextAccessor = httpContextAccessor;
            _motorcycleService = motorcycleService;
            _driverService = driverService;
            _planService = planService;
            _logger = logger;
        }

        public async Task<List<RentDTO>> GetAllAsync()
        {
            var model = _repository.GetAll();
            return await RentDTO.Convert(model);
        }

        public async Task<RentDTO> GetByIdAsync(string id)
        {
            RentModel model = await _repository.GetById(id);
            if (model != null)
                return await RentDTO.Convert(model);

            _logger.LogInformation($"Rent with Id = {id} not found");
            return null;
        }

        public async Task<List<RentDTO>> GetByDriverIdAsync(string driverId)
        {
            var model = _repository.GetByDriverId(driverId);
            if (model != null)
                return await RentDTO.Convert(model);

            _logger.LogDebug($"Rent with DriverId = {driverId} not found");
            return null;
        }

        public async Task<Task<RentDTO>> CreateAsync(string id)
        {
            var nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DriverModel driver = await _driverService.GetByIdModel(nameIdentifier);

            if (driver.CnhType == CnhTypeEnum.B)
            {
                _logger.LogError("Cnh must be A or AB");
                throw new Exception("Cnh must be A or AB");
            }

            var motorcycle = await _motorcycleService.GetFirstAvailableAsync();
            if (motorcycle == null)
            {
                _logger.LogError("No motorbikes available");
                throw new Exception("No motorbikes available");
            }
            DateOnly startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            PlanModel plan = await _planService.GetByIdModel(id);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(plan.TotalDay + 1));

            await _motorcycleService.UpdateStatusAsync(motorcycle.Id, MotorcycleStatusEnum.Unavailable);

            RentModel rentModel = RentRequest.Convert(driver.Id, motorcycle.Id, plan.Id, startDate, endDate);
            await _repository.CreateAsync(rentModel);
            return RentDTO.Convert(rentModel);
        }

        public async Task<bool> UpdateAsync(string id)
        {
            RentModel rent = await GetByIdModel(id);

            if (rent.TotalPrice != 0)
            {
                _logger.LogError("Operation has already been completed, cannot be updated");
                throw new Exception("Operation has already been completed, cannot be updated");
            }

            DateOnly returnDate = DateOnly.FromDateTime(DateTime.UtcNow);
            PlanModel plan = await _planService.GetByIdModel(rent.PlanId);
            _motorcycleService.UpdateStatusAsync(rent.MotorcycleId, MotorcycleStatusEnum.Available);

            if (returnDate > rent.EndDate)
            {
                int delayDays = returnDate.DayNumber - rent.EndDate.DayNumber;
                Decimal totalPrice = ((plan.Price * (plan.PenaltyPercentage / 100) * delayDays) + (delayDays * 50)) + plan.Price;

                rent = RentRequest.ConvertUpdate(rent, returnDate, plan.Price);
                await _repository.UpdateAsync(id, rent);
                return true;
            }

            rent = RentRequest.ConvertUpdate(rent, returnDate, plan.Price);
            await _repository.UpdateAsync(id, rent);
            return true;
        }

        public async Task<RentModel> GetByIdModel(string id)
        {
            return await _repository.GetById(id) ?? throw new Exception($"Rent not found"); ;
        }

        public async Task<List<RentModel>> GetByDriverIdModel(string driverId)
        {
            return await _repository.GetByDriverId(driverId) ?? throw new Exception("Delivery not found"); ;
        }
    }
}