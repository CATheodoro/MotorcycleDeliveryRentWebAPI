using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class DeliveryRequest
    {
        public string? Id { get; set; }
        public string? AdminId { get; set; }
        public string? DriverId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime AcceptDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal? Price { get; set; }
        public DeliveryStatusEnum Status { get; set; }

        internal static DeliveryModel Convert(string adminId, DateTime creationDate, decimal price, DeliveryStatusEnum status)
        {
            DeliveryModel model = new DeliveryModel();
            model.AdminId = adminId;
            model.CreationDate = creationDate;
            model.Price = price;
            model.Status = status;
            return model;
        }

        internal static DeliveryModel ConvertAccept(DeliveryModel model, DateTime acceptDate, DeliveryStatusEnum status)
        {
            model.AcceptDate = acceptDate;
            model.Status = status;
            return model;
        }

        internal static DeliveryModel ConvertDelivery(DeliveryModel model, DateTime deliveryDate, DeliveryStatusEnum status)
        {
            model.DeliveryDate = deliveryDate;
            model.Status = status;
            return model;
        }
    }
}
