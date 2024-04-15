using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Api.Validators;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _repository;
        private readonly IRentRepository _rentRepository;
        private readonly ILogger<MotorcycleModel> _logger;

        public MotorcycleService(IMotorcycleRepository repository, IRentRepository rentRepository, ILogger<MotorcycleModel> logger)
        {
            _repository = repository;
            _rentRepository = rentRepository;
            _logger = logger;
        }

        public async Task<List<MotorcycleDTO>> GetAllAsync()
        {
            var model = _repository.GetAll();
            return await MotorcycleDTO.Convert(model);
        }

        public async Task<MotorcycleDTO> GetFirstAvailableAsync()
        {
            var model = await _repository.GetFirstAvailable();
            if (model == null)
                throw new Exception("Don't have an available motorcycle");
            return await MotorcycleDTO.Convert(model);
        }

        public async Task<MotorcycleDTO> GetByIdAsync(string id)
        {
            var model = await _repository.GetById(id);
            if (model != null)
                return await MotorcycleDTO.Convert(model);

            _logger.LogDebug($"Motorcycle with Id = {id} not found");
            return null;
        }

        public async Task<MotorcycleDTO> GetByPlateAsync(string plate)
        {
            ValidatorAdminDriver.Plate(plate);
            var model = await _repository.GetByPlate(plate);
            if (model != null)
                return await MotorcycleDTO.Convert(model);

            _logger.LogDebug($"Motorcycle with plate = {plate} not found");
            return null;
        }

        public async Task<MotorcycleDTO> CreateAsync(MotorcycleRequest request)
        {
            ValidatorAdminDriver.Plate(request.Plate);
            var modelPlate = await _repository.GetByPlate(request.Plate);
            if (modelPlate != null)
            {
                _logger.LogError($"The plate must be unique. Informed plate = {request.Plate}");
                throw new Exception($"The plate must be unique. Informed plate = {request.Plate}");
            }

            MotorcycleModel model = MotorcycleRequest.Convert(request);
            await _repository.Create(model);
            return await MotorcycleDTO.Convert(model);
        }

        public async Task<bool> UpdateAsync(string id, MotorcycleUpdateRequest request)
        {
            var model = await GetByIdModel(id);

            model = MotorcycleUpdateRequest.Convert(model, request);
            await _repository.Update(id, model);
            return true;
        }

        public async Task<bool> UpdatePlateAsync(string id, string plate)
        {
            ValidatorAdminDriver.Plate(plate);
            var modelPlate = await _repository.GetByPlate(plate);
            if (modelPlate != null)
            {
                _logger.LogError($"The plate must be unique. Informed plate = {plate}");
                throw new Exception($"The plate must be unique. Informed plate = {plate}");
            }

            var model = await GetByIdModel(id);

            model.Plate = plate;
            await _repository.Update(id, model);
            return true;
        }

        public async Task<bool> UpdateStatusAsync(string id, MotorcycleStatusEnum status)
        {
            var model = await GetByIdModel(id);

            model.Status = status;
            await _repository.Update(id, model);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            RentModel model = await _rentRepository.GetByMotorcycleId(id);
            if (model != null)
            {
                _logger.LogInformation($"Motorcycle with id = {id} cannot be deleted as it already has a plan registration");
                throw new Exception($"Motorcycle with id = {id} cannot be deleted as it already has a plan registration");
            }
            await _repository.Delete(id);
            return true;
        }

        public async Task<MotorcycleModel> GetByIdModel(string id)
        {
            return await _repository.GetById(id) ?? throw new Exception($"Motorcycle with Id = {id} not found"); ;
        }

        public async Task<MotorcycleModel> GetByPlateModel(string plate)
        {
            ValidatorAdminDriver.Plate(plate);
            return await _repository.GetByPlate(plate) ?? throw new Exception($"Motorcycle with plate = {plate} not found"); ;
        }
    }
}