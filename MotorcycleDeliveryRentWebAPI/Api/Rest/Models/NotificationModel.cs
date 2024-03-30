using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class NotificationModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("message")]
        public required string Message { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
