using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Models
{
    public class CnhImageModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fileName")]
        [BsonRequired]
        public string FileName { get; set; }

        [BsonElement("filePath")]
        [BsonRequired]
        public string FilePath { get; set; }

        [BsonElement("fileSize")]
        [BsonRequired]
        public long FileSize { get; set; }
    }
}
