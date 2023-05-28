using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Shortener.Models;
public class UrlMapping
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get; set;}
    public string? LongUrl { get; set;}
    public string ShortUrl { get; set;} = null!;
    public List<DateTime>? AccessTimestamps { get; set; }
}
