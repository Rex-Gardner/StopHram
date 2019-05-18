using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.Identity.Mongo.Model;

namespace Models.Users
{
    public class User : MongoUser
    {
        public IReadOnlyList<Guid> CreatedTroubles { get; set; }
        public IReadOnlyList<Guid> LikedTroubles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
