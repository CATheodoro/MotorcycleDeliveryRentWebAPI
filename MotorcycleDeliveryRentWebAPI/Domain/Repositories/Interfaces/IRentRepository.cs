using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface IRentRepository
    {
        public Task<List<RentModel>> GetAll();
        public Task<RentModel> GetById(string id);
        public Task<List<RentModel>> GetByDriverId(string driverId);
        public Task<RentModel> GetByMotorcycleId(string motorcycleId);
        public Task CreateAsync(RentModel model);
        public Task UpdateAsync(string id, RentModel model);
    }
}
