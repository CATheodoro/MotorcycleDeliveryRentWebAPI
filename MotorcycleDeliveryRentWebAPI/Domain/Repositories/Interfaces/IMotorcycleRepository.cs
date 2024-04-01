using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface IMotorcycleRepository
    {
        public Task<List<MotorcycleModel>> GetAll();
        public Task<MotorcycleModel> GetFirstAvailable();
        public Task<MotorcycleModel> GetById(string id);
        public Task<MotorcycleModel> GetByPlate(string plate);
        public Task Create(MotorcycleModel model);
        public Task Update(string id, MotorcycleModel model);
        public Task Delete(string id);
    }
}
