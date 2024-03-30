using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;
using System.Security.Claims;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class RentService : IRentService
    {
        private readonly IMongoCollection<RentModel> _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMotorcycleService _motorcycleService;
        private readonly IDriverService _driverService;
        private readonly IPlanService _planService;
        private readonly ILogger<RentModel> _logger;

        public RentService(IMongoDBSettings settings, IMongoClient mongoClient, IHttpContextAccessor httpContextAccessor,
            IMotorcycleService motorcycleService, IDriverService driverService, IPlanService planService, ILogger<RentModel> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<RentModel>("Rent");
            _httpContextAccessor = httpContextAccessor;
            _motorcycleService = motorcycleService;
            _driverService = driverService;
            _planService = planService;
            _logger = logger;
        }

        public List<RentDTO> GetAll()
        {
            return RentDTO.Convert(_context.Find(x => true).ToList());
        }

        public RentDTO GetById(string id)
        {
            RentModel model = _context.Find(x => x.Id == id).FirstOrDefault();
            if (model != null)
                return RentDTO.Convert(model);

            _logger.LogInformation($"Rent with Id = {id} not found");
            return null;
        }

        public List<RentDTO> GetByDriverId(string driverId)
        {
            List<RentModel> model = _context.Find(x => x.DriverId == driverId).ToList();
            if (model != null)
                return RentDTO.Convert(model);

            _logger.LogDebug($"Rent with DriverId = {driverId} not found");
            return null;
        }

        public RentDTO Create(string id)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel driver = _driverService.GetByEmailModel(email);

            if (driver.CnhType == CnhTypeEnum.B)
            {
                _logger.LogError("Cnh must be A or AB");
                throw new Exception("Cnh must be A or AB");
            }

            MotorcycleModel motorcycle = _motorcycleService.GetAvailableModel();
            if (motorcycle == null)
            {
                _logger.LogError("No motorbikes available");
                throw new Exception("No motorbikes available");
            }
            DateOnly startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            PlanModel plan = _planService.GetByIdModel(id);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(plan.TotalDay + 1));

            _motorcycleService.UpdateStatus(motorcycle.Id, MotorcycleStatusEnum.Unavailable);

            RentModel rentModel = RentRequest.Convert(driver.Id, motorcycle.Id, plan.Id, startDate, endDate);
            _context.InsertOne(rentModel);
            return RentDTO.Convert(rentModel);
        }

        public bool Update(string id)
        {
            RentModel rent = GetByIdModel(id);

            if (rent.TotalPrice != 0)
            {
                _logger.LogError("Operation has already been completed, cannot be updated");
                throw new Exception("Operation has already been completed, cannot be updated");
            }

            DateOnly returnDate = DateOnly.FromDateTime(DateTime.UtcNow);
            PlanModel plan = _planService.GetByIdModel(rent.PlanId);
            _motorcycleService.UpdateStatus(rent.MotorcycleId, MotorcycleStatusEnum.Available);

            if (returnDate > rent.EndDate)
            {
                int delayDays = returnDate.DayNumber - rent.EndDate.DayNumber;
                Decimal totalPrice = ((plan.Price * (plan.PenaltyPercentage / 100) * delayDays) + (delayDays * 50)) + plan.Price;

                rent = RentRequest.ConvertUpdate(rent, returnDate, plan.Price);
                _context.ReplaceOne(x => x.Id == id, rent);
                return true;
            }

            rent = RentRequest.ConvertUpdate(rent, returnDate, plan.Price);
            _context.ReplaceOne(x => x.Id == id, rent);
            return true;
        }

        public RentModel GetByIdModel(string id)
        {
            return _context.Find(x => x.Id == id).FirstOrDefault() ?? throw new Exception($"Rent not found"); ;
        }

        public List<RentModel> GetByDriverIdModel(string driverId)
        {
            return _context.Find(x => x.DriverId == driverId).ToList() ?? throw new Exception("Delivery not found"); ;
        }
    }
}