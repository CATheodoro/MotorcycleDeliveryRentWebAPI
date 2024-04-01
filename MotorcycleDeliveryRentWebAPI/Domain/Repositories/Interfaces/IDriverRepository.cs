using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        public Task<List<DriverModel>> GetAll();
        public Task<DriverModel> GetById(string id);
        public Task<DriverModel> GetByEmail(string email);
        public Task<DriverModel> GetByCnh(string cnh);
        public Task<DriverModel> GetByCnpj(string cnpj);
        public Task<List<DriverModel>> GetByStatus(DriverStatusEnum status);
        public Task CreateAsync(DriverModel model);
        public Task UpdateAsync(string id, DriverModel model);
    }
}
