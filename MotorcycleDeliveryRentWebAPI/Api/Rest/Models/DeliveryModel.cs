using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class DeliveryModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("AdminId")]
        [BsonRequired]
        public string? AdminId { get; set; }

        [BsonElement("driverId")]
        public string? DriverId { get; set; }

        [BsonElement("creationDate")]
        public DateTime CreationDate { get; set; }

        [BsonElement("acceptDate")]
        public DateTime AcceptDate { get; set; }

        [BsonElement("deliveryDate")]
        public DateTime DeliveryDate { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("status")]
        public DeliveryStatusEnum Status { get; set; }
    }
}
