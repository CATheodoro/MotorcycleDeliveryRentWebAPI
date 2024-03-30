using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class PlanService : IPlanService
    {
        private readonly IMongoCollection<PlanModel> _context;
        private readonly ILogger<PlanModel> _logger;

        public PlanService(IMongoDBSettings settings, IMongoClient mongoClient, ILogger<PlanModel> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<PlanModel>("Plan");
            _logger = logger;
        }

        public List<PlanDTO> GetAll()
        {
            return PlanDTO.Convert(_context.Find(x => true).ToList());
        }

        public PlanDTO GetById(string id)
        {
            PlanModel model = _context.Find(x => x.Id == id).FirstOrDefault();
            if (model != null)
                return PlanDTO.Convert(model);

            _logger.LogDebug($"Plan with Id = {id} not found");
            return null;
        }

        public PlanDTO Create(PlanRequest request)
        {
            PlanModel model = PlanRequest.Convert(request);
            _context.InsertOne(model);
            return PlanDTO.Convert(model);
        }

        public bool Update(string id, PlanRequest request)
        {
            PlanModel model = GetByIdModel(id);
            model = PlanRequest.ConvertUpdate(model, request);
            _context.ReplaceOne(x => x.Id == id, model);
            return true;

        }

        public PlanModel GetByIdModel(string id)
        {
            return _context.Find(x => x.Id == id).FirstOrDefault() ?? throw new Exception($"Plan with Id = {id} not found"); ;
        }
    }
}