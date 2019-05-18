using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Tags
{
    public class Tag
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
        
        [BsonElement("Name")]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
    }
}