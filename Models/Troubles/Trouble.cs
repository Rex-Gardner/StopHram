using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Device.Location;

namespace Models.Troubles
{
    public class Trouble
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        [BsonElement("Name")]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
        
        [BsonElement("Description")]
        [BsonRepresentation(BsonType.String)]
        public string Description { get; set; }
        
        [BsonElement("Images")]
        public IReadOnlyList<string> Images { get; set; }
        
        [BsonElement("Coordinates")]
        public GeoCoordinate Coordinates { get; set; }
        
        [BsonElement("Address")]
        [BsonRepresentation(BsonType.String)]
        public string Address { get; set; }
        
        [BsonElement("Tags")]
        public IReadOnlyList<string> Tags { get; set; }

        [BsonElement("Status")]
        [BsonRepresentation(BsonType.String)]
        public TroubleStatus Status { get; set; }
        
        [BsonElement("Author")]
        [BsonRepresentation(BsonType.String)]
        public string Author { get; set; }
        
        [BsonElement("LikedUsers")]
        [BsonRepresentation(BsonType.String)]
        public IReadOnlyList<string> LikedUsers { get; set; }
        
        [BsonElement("CreatedAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
        
        [BsonElement("LastUpdateAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdateAt { get; set; }
    }
}