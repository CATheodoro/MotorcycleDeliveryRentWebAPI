using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IDeliveryService
    {
        public Task<List<DeliveryDTO>> GetAllAsync();
        public Task<DeliveryDTO> GetByIdAsync(string id);
        public Task<DeliveryDTO> CreateAsync(decimal price);
        public Task<bool> AcceptAsync(string id);
        public Task<bool> DeliveryAsync(string id);
        public Task<bool> CancelAsync(string id);
        public Task<DeliveryModel> GetByIdModel(string id);
    }
}
