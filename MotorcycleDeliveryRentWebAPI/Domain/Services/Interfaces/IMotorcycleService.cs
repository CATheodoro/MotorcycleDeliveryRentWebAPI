using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IMotorcycleService
    {
        public List<MotorcycleDTO> GetAll();
        public MotorcycleDTO GetAvailable();
        public MotorcycleDTO GetById(string id);
        public MotorcycleDTO GetByPlate(string plate);
        public MotorcycleDTO Create(MotorcycleRequest request);
        public bool Update(string id, MotorcycleUpdateRequest request);
        public bool UpdatePlate(string id, string plate);
        public bool UpdateStatus(string id, MotorcycleStatusEnum status);
        public bool Delete(string id);
        public MotorcycleModel GetAvailableModel();
    }
}
