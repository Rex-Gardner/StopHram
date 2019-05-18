using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels.Users
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IReadOnlyList<string> Roles { get; set; }
        public IReadOnlyList<string> CreatedTroubles { get; set; }
        public IReadOnlyList<string> LikedTroubles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
