using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        public Task<List<AdminModel>> GetAll();
        public Task<AdminModel> GetById(string id);
        public Task<AdminModel> GetByEmail(string email);
        public Task CreateAsync(AdminModel model);
        public Task UpdateAsync(string id, AdminModel model);
    }
}
