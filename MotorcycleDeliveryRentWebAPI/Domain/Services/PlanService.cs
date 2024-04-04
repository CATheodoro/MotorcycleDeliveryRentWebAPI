using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _repository;
        private readonly ILogger<PlanService> _logger;

        public PlanService(IPlanRepository planRepository, ILogger<PlanService> logger)
        {
            _repository = planRepository;
            _logger = logger;
        }

        public async Task<List<PlanDTO>> GetAllAsync()
        {
            var model = _repository.GetAll();
            return await PlanDTO.Convert(model);
        }

        public async Task<PlanDTO> GetByIdAsync(string id)
        {
            var model = await _repository.GetById(id);
            if (model != null)
                return await PlanDTO.Convert(model);

            _logger.LogDebug($"Plan with Id = {id} not found");
            return null;
        }

        public async Task<PlanDTO> CreateAsync(PlanRequest request)
        {
            PlanModel model = PlanRequest.Convert(request);
            _repository.CreateAsync(model);
            return await PlanDTO.Convert(model);
        }

        public async Task<bool> UpdateAsync(string id, PlanRequest request)
        {
            PlanModel model = await GetByIdModel(id);
            model = PlanRequest.ConvertUpdate(model, request);
            await _repository.UpdateAsync(id, model);
            return true;

        }

        public async Task<PlanModel> GetByIdModel(string id)
        {
            return await _repository.GetById(id) ?? throw new Exception($"Plan with Id = {id} not found"); ;
        }
    }
}