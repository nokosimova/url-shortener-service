
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinkShortener.Models
{
    public class Link
    { 
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string LinkName  { get; set; } = null!;
        public string ShortName  { get; set; } = null!;
        public string CookieValue { get; set; } = null!;
        public long VisitsCount { get; set; }
    }
}