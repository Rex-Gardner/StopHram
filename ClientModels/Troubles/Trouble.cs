using System;
using System.Collections.Generic;

namespace ClientModels.Troubles
{
    public class Trouble
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<string> Images { get; set; }
        public IReadOnlyList<double> Coordinates { get; set; }
        public string Address { get; set; }
        public IReadOnlyList<string> Tags { get; set; }
        public string Status { get; set; }
        public string Author { get; set; }
        public IReadOnlyList<string> LikedUsers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}