using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class AdminModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        [BsonRequired]
        public string? Email { get; set; }

        [BsonElement("password")]
        [BsonRequired]
        public string? Password { get; set; }
    }
}
