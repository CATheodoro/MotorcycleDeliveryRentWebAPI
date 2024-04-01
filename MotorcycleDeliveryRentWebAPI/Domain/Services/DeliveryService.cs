using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;
using System.Security.Claims;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminService _adminService;
        private readonly IDriverService _driverService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<DeliveryModel> _logger;

        public DeliveryService(IDeliveryRepository deliveryRepository, IHttpContextAccessor httpContextAccessor,IAdminService adminService, IDriverService driverService,
            INotificationService notificationService, ILogger<DeliveryModel> logger)
        {
            _repository = deliveryRepository;
            _httpContextAccessor = httpContextAccessor;
            _adminService = adminService;
            _driverService = driverService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<List<DeliveryDTO>> GetAllAsync()
        {
            var model = _repository.GetAll();
            return await DeliveryDTO.Convert(model);
        }

        public async Task<DeliveryDTO> GetByIdAsync(string id)
        {
            DeliveryModel model = await _repository.GetById(id);
            if (model != null)
                return await DeliveryDTO.Convert(model);

            _logger.LogDebug($"Delivery id = {id} not found");
            return null;
        }

        //public List<DeliveryDTO> GetByDriverNotification()
        //{
        //    return DeliveryDTO.Convert(_notificationService.(x => true).ToList());
        //}

        public async Task<DeliveryDTO> CreateAsync(decimal price)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            Task<AdminModel> admin = _adminService.GetByEmailModel(email);
            DeliveryModel model = DeliveryRequest.Convert(admin.Result.Id, DateTime.UtcNow, price, DeliveryStatusEnum.Available);

            _notificationService.PublishNewDeliveryNotification(model.Id);
            await _repository.CreateAsync(model);
            return await DeliveryDTO.Convert(model);
        }

        public async Task<bool> AcceptAsync(string id)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel driver = await _driverService.GetByEmailModel(email);

            DeliveryModel model = await GetByIdModel(id);

            if (model.Id != id)
            {
                _logger.LogError("Delivery has different driver");
                throw new Exception("Delivery has different driver");
            }

            if (driver.Status == DriverStatusEnum.Unavailable)
            {
                _logger.LogError("Driver must be available");
                throw new Exception("Driver must be available");
            }
            _notificationService.PublishDeliveryAcceptance(model.Id, driver.Id);
            _driverService.UpdateStatus(driver.Id, DriverStatusEnum.Unavailable);

            model = DeliveryRequest.ConvertAccept(model, driver.Id, DateTime.UtcNow, DeliveryStatusEnum.Accepted);
            await _repository.UpdateAsync(id, model);
            return true;
        }

        public async Task<bool> DeliveryAsync(string id)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel driver = await _driverService.GetByEmailModel(email);

            DeliveryModel model = await GetByIdModel(id);

            if (model.DriverId != driver.Id)
            {
                _logger.LogError("Different drivers");
                throw new Exception("Different drivers");
            }

            _driverService.UpdateStatus(driver.Id, DriverStatusEnum.Available);

            model = DeliveryRequest.ConvertDelivery(model, DateTime.UtcNow, DeliveryStatusEnum.Delivered);
            await _repository.UpdateAsync(id, model);
            return true;
        }

        public async Task<bool> CancelAsync(string id)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            Task<AdminModel> admin = _adminService.GetByEmailModel(email);

            DeliveryModel model = await GetByIdModel(id);
            if (model.AdminId != admin.Result.Id)
            {
                _logger.LogError("Different users");
                throw new Exception("Different users");
            }

            if (model.Status == DeliveryStatusEnum.Delivered || model.Status == DeliveryStatusEnum.Canceled)
            {
                _logger.LogInformation("A race has already ended or been canceled");
                return false;
            }

            _driverService.UpdateStatus(model.DriverId, DriverStatusEnum.Available);

            model = DeliveryRequest.ConvertDelivery(model, DateTime.UtcNow, DeliveryStatusEnum.Canceled);
            await _repository.UpdateAsync(id, model);
            return true;
        }

        public async Task<DeliveryModel> GetByIdModel(string id)
        {
            return await _repository.GetById(id) ?? throw new Exception($"Delivery id = {id} not found");
        }

        //public async Task<List<DeliveryModel>> GetByDriverNotificationModel()
        //{
        //    return await _repository.GetAll();
        //}
    }
}