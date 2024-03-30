using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IPlanService
    {
        public List<PlanDTO> GetAll();
        public PlanDTO GetById(string id);
        public PlanDTO Create(PlanRequest request);
        public bool Update(string id, PlanRequest request);
        public PlanModel GetByIdModel(string id);
    }
}
