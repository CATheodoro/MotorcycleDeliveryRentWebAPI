using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface IDeliveryRepository
    {
        public Task<List<DeliveryModel>> GetAll();
        public Task<DeliveryModel> GetById(string id);
        public Task CreateAsync(DeliveryModel model);
        public Task UpdateAsync(string id, DeliveryModel model);
    }
}
