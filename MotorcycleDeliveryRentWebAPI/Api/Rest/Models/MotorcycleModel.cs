using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class MotorcycleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("year")]
        [BsonRequired]
        public int? Year { get; set; }

        [BsonElement("model")]
        [BsonRequired]
        public string? Model { get; set; }

        [BsonElement("plate")]
        [BsonRequired]
        public string? Plate { get; set; }

        [BsonElement("status")]
        public MotorcycleStatusEnum Status { get; set; }
    }
}
