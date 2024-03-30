using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class PlanModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("year")]
        [BsonRequired]
        public decimal Price { get; set; }

        [BsonElement("model")]
        [BsonRequired]
        public string? Description { get; set; }

        [BsonElement("totalDay")]
        [BsonRequired]
        public int TotalDay { get; set; }

        [BsonElement("penaltyPercentage")]
        [BsonRequired]
        public int PenaltyPercentage { get; set; }
    }
}
