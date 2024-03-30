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
    public class DeliveryService : IDeliveryService
    {
        private readonly IMongoCollection<DeliveryModel> _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminService _adminService;
        private readonly IDriverService _driverService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<DeliveryModel> _logger;

        public DeliveryService(IMongoDBSettings settings, IMongoClient mongoClient, IHttpContextAccessor httpContextAccessor,
            IAdminService adminService, IDriverService driverService, INotificationService notificationService, ILogger<DeliveryModel> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<DeliveryModel>("Delivery");
            _httpContextAccessor = httpContextAccessor;
            _adminService = adminService;
            _driverService = driverService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public List<DeliveryDTO> GetAll()
        {
            return DeliveryDTO.Convert(_context.Find(x => true).ToList()); ;
        }

        public DeliveryDTO GetById(string id)
        {
            DeliveryModel model = _context.Find(x => x.Id == id).FirstOrDefault();
            if (model != null)
                return DeliveryDTO.Convert(model);

            _logger.LogDebug($"Delivery id = {id} not found");
            return null;
        }

        public List<DeliveryDTO> GetByDriverNotification()
        {
            return DeliveryDTO.Convert(_context.Find(x => true).ToList());
        }

        public DeliveryDTO Create(decimal price)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            AdminModel admin = _adminService.GetByEmailModel(email);
            DeliveryModel delivery = DeliveryRequest.Convert(admin.Id, DateTime.UtcNow, price, DeliveryStatusEnum.Available);

            _notificationService.PublishNewDeliveryNotification(delivery.Id);
            _context.InsertOne(delivery);

            return DeliveryDTO.Convert(delivery);
        }

        public bool Accept(string id)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel driver = _driverService.GetByEmailModel(email);

            DeliveryModel delivery = GetByIdModel(id);

            if (delivery.Id != id)
            {
                _logger.LogError("Delivery has different driver");
                throw new Exception("Delivery has different driver");
            }

            if (driver.Status == DriverStatusEnum.Unavailable)
            {
                _logger.LogError("Driver must be available");
                throw new Exception("Driver must be available");
            }
            _notificationService.PublishDeliveryAcceptance(delivery.Id, driver.Id);
            _driverService.UpdateStatus(driver.Id, DriverStatusEnum.Unavailable);

            delivery = DeliveryRequest.ConvertAccept(delivery, DateTime.UtcNow, DeliveryStatusEnum.Accepted);
            _context.ReplaceOne(x => x.Id == id, delivery);
            return true;
        }

        public bool Delivery(string id)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel driver = _driverService.GetByEmailModel(email);

            DeliveryModel delivery = GetByIdModel(id);

            if (delivery.DriverId != driver.Id)
            {
                _logger.LogError("Different drivers");
                throw new Exception("Different drivers");
            }

            _driverService.UpdateStatus(driver.Id, DriverStatusEnum.Available);

            delivery = DeliveryRequest.ConvertDelivery(delivery, DateTime.UtcNow, DeliveryStatusEnum.Delivered);
            _context.ReplaceOne(x => x.Id == id, delivery);
            return true;
        }

        public bool Cancel(string id)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            AdminModel admin = _adminService.GetByEmailModel(email);

            DeliveryModel delivery = GetByIdModel(id);
            if (delivery.AdminId != admin.Id)
            {
                _logger.LogError("Different users");
                throw new Exception("Different users");
            }
            if (delivery.Status != DeliveryStatusEnum.Delivered)
            {
                _logger.LogError("Cannot cancel finished race");
                throw new Exception("Cannot cancel finished race");
            }

            _driverService.UpdateStatus(delivery.DriverId, DriverStatusEnum.Available);
            delivery = DeliveryRequest.ConvertDelivery(delivery, DateTime.UtcNow, DeliveryStatusEnum.Canceled);
            _context.ReplaceOne(x => x.Id == id, delivery);
            return true;
        }

        public DeliveryModel GetByIdModel(string id)
        {
            return _context.Find(x => x.Id == id).FirstOrDefault() ?? throw new Exception($"Delivery id = {id} not found");
        }

        public List<DeliveryModel> GetByDriverNotificationModel()
        {
            return _context.Find(x => true).ToList();
        }
    }
}