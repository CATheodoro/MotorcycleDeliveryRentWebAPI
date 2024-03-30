using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class RentModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("driverId")]
        [BsonRequired]
        public string? DriverId { get; set; }

        [BsonElement("motorcycleId")]
        [BsonRequired]
        public string? MotorcycleId { get; set; }

        [BsonElement("planId")]
        [BsonRequired]
        public string? PlanId { get; set; }

        [BsonElement("startDate")]
        public DateOnly StartDate { get; set; }

        [BsonElement("endDate")]
        public DateOnly EndDate { get; set; }

        [BsonElement("returnDate")]
        public DateOnly ReturnDate { get; set; }

        [BsonElement("totalPrice")]
        public decimal TotalPrice { get; set; }
    }
}
