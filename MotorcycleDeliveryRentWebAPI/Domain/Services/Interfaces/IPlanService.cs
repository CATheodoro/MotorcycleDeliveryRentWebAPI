using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IPlanService
    {
        public Task<List<PlanDTO>> GetAllAsync();
        public Task<PlanDTO> GetByIdAsync(string id);
        public Task<PlanDTO> CreateAsync(PlanRequest request);
        public Task<bool> UpdateAsync(string id, PlanRequest request);
        public Task<PlanModel> GetByIdModel(string id);
    }
}
