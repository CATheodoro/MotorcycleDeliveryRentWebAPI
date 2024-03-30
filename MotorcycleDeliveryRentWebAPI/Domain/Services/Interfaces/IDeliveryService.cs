using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IDeliveryService
    {
        public List<DeliveryDTO> GetAll();
        public DeliveryDTO GetById(string id);
        public List<DeliveryDTO> GetByDriverNotification();
        public DeliveryDTO Create(decimal price);
        public bool Accept(string id);
        public bool Delivery(string id);
        public bool Cancel(string id);
    }
}
