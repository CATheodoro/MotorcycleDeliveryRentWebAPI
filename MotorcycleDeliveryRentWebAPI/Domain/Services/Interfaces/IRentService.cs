using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IRentService
    {
        public List<RentDTO> GetAll();
        public RentDTO GetById(string id);
        public List<RentDTO> GetByDriverId(string driverId);
        public RentDTO Create(string id);
        public bool Update(string id);
    }
}
