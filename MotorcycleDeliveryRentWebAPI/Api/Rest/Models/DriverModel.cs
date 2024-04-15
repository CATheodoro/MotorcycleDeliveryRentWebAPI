using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class DriverModel
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

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("cnpj")]
        [BsonRequired]
        public string? Cnpj { get; set; }

        [BsonElement("birthDate")]
        public DateOnly BirthDate { get; set; }

        [BsonElement("cnh")]
        [BsonRequired]
        public string? Cnh { get; set; }

        [BsonElement("cnhType")]
        public CnhTypeEnum CnhType { get; set; }

        [BsonElement("status")]
        public DriverStatusEnum Status { get; set; }

        [BsonElement("cnhImageId")]
        public string? CnhImageId { get; set; }

        [BsonElement("rule")]
        public List<string>? Rule { get; set; }
    }
}
