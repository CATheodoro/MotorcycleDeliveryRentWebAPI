using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IRentService
    {
        public Task<List<RentDTO>> GetAllAsync();
        public Task<RentDTO> GetByIdAsync(string id);
        public Task<List<RentDTO>> GetByDriverIdAsync(string driverId);
        public Task<RentDTO> CreateAsync(string id);
        public Task<bool> UpdateAsync(string id);
        public Task<RentModel> GetByIdModel(string id);
        public Task<List<RentModel>> GetByDriverIdModel(string driverId);
    }
}
