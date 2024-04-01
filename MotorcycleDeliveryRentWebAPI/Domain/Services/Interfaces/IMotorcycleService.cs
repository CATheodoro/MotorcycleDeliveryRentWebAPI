using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IMotorcycleService
    {
        public Task<List<MotorcycleDTO>> GetAllAsync();
        public Task<MotorcycleDTO> GetFirstAvailableAsync();
        public Task<MotorcycleDTO> GetByIdAsync(string id);
        public Task<MotorcycleDTO> GetByPlateAsync(string plate);
        public Task<MotorcycleDTO> CreateAsync(MotorcycleRequest request);
        public Task<bool> UpdateAsync(string id, MotorcycleUpdateRequest request);
        public Task<bool> UpdatePlateAsync(string id, string plate);
        public Task<bool> UpdateStatusAsync(string id, MotorcycleStatusEnum status);
        public Task<bool> DeleteAsync(string id);
        public Task<MotorcycleModel> GetByIdModel(string id);
        public Task<MotorcycleModel> GetByPlateModel(string plate);
    }
}
