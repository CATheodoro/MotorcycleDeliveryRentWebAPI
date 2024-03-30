using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Api.Validators;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMongoCollection<MotorcycleModel> _context;
        private readonly IMongoCollection<RentModel> _contextRent;
        private readonly ILogger<MotorcycleModel> _logger;

        public MotorcycleService(IMongoDBSettings settings, IMongoClient mongoClient, ILogger<MotorcycleModel> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<MotorcycleModel>("Motorcycle");
            _contextRent = database.GetCollection<RentModel>("Rent");
            _logger = logger;
        }

        public List<MotorcycleDTO> GetAll()
        {
            return MotorcycleDTO.Convert(_context.Find(x => true).ToList());
        }

        public MotorcycleDTO GetAvailable()
        {
            return MotorcycleDTO.Convert(_context.Find(x => x.Status == MotorcycleStatusEnum.Available).FirstOrDefault());
        }

        public MotorcycleDTO GetById(string id)
        {
            MotorcycleModel model = _context.Find(x => x.Id == id).FirstOrDefault();
            if (model != null)
                return MotorcycleDTO.Convert(model);

            _logger.LogDebug($"Motorcycle with Id = {id} not found");
            return null;
        }

        public MotorcycleDTO GetByPlate(string plate)
        {
            ValidatorAdminDriver.Plate(plate);
            MotorcycleModel model = _context.Find(x => x.Plate == plate).FirstOrDefault();
            if (model != null)
                return MotorcycleDTO.Convert(model);

            _logger.LogDebug($"Motorcycle with plate = {plate} not found");
            return null;
        }

        public MotorcycleDTO Create(MotorcycleRequest request)
        {
            CheckPlate(request.Plate);
            MotorcycleModel model = MotorcycleRequest.Convert(request);
            _context.InsertOne(model);
            return MotorcycleDTO.Convert(model);
        }

        public bool Update(string id, MotorcycleUpdateRequest request)
        {
            MotorcycleModel model = GetByIdModel(id);

            model = MotorcycleUpdateRequest.Convert(model, request);
            _context.ReplaceOne(x => x.Id == id, model);
            return true;
        }

        public bool UpdatePlate(string id, string plate)
        {
            CheckPlate(plate);
            MotorcycleModel model = GetByIdModel(id);

            model.Plate = plate;
            _context.ReplaceOne(x => x.Id == id, model);
            return true;

        }

        public bool UpdateStatus(string id, MotorcycleStatusEnum status)
        {
            MotorcycleModel model = GetByIdModel(id);

            model.Status = status;
            _context.ReplaceOne(x => x.Id == id, model);
            return true;
        }

        public bool Delete(string id)
        {
            List<RentModel> model = _contextRent.Find(x => x.MotorcycleId == id).ToList();
            if (model != null)
            {
                _logger.LogInformation($"Motorcycle with id = {id} cannot be deleted as it already has a plan registration");
                throw new Exception($"Motorcycle with id = {id} cannot be deleted as it already has a plan registration");
            }
            _context.DeleteOne(x => x.Id == id);
            return true;
        }

        public void CheckPlate(string plate)
        {
            ValidatorAdminDriver.Plate(plate);
            if (_context.Find(x => x.Plate == plate).FirstOrDefault() != null)
            {
                _logger.LogError($"The plate must be unique. Informed plate = {plate}");
                throw new Exception($"The plate must be unique. Informed plate = {plate}");
            }
        }

        public MotorcycleModel GetAvailableModel()
        {
            return _context.Find(x => x.Status == MotorcycleStatusEnum.Available).FirstOrDefault();
        }

        public MotorcycleModel GetByIdModel(string id)
        {
            return _context.Find(x => x.Id == id).FirstOrDefault() ?? throw new Exception($"Motorcycle with Id = {id} not found"); ;
        }

        public MotorcycleModel GetByPlateModel(string plate)
        {
            ValidatorAdminDriver.Plate(plate);
            return _context.Find(x => x.Plate == plate).FirstOrDefault() ?? throw new Exception($"Motorcycle with plate = {plate} not found"); ;
        }
    }
}