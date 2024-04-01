using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        public Task<List<PlanModel>> GetAll();
        public Task<PlanModel> GetById(string id);
        public Task CreateAsync(PlanModel model);
        public Task UpdateAsync(string id, PlanModel model);
    }
}
