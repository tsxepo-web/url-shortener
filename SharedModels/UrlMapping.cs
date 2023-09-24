using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedModels
{
    public class UrlMapping
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;

    }
}
