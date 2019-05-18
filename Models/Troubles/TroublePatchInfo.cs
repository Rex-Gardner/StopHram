using System;
using System.Collections.Generic;
using System.Device.Location;

namespace Models.Troubles
{
    public class TroublePatchInfo
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<string> Images { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
        public IReadOnlyList<string> Tags { get; set; }
        public TroubleStatus? Status { get; set; }

        public TroublePatchInfo(Guid id, string name = null, string description = null, 
            IReadOnlyList<string> images = null, double? latitude = null, double? longitude = null, 
            string address = null, IReadOnlyList<string> tags = null, TroubleStatus? status = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Images = images;
            Latitude = latitude;
            Longitude = longitude;
            Address = address;
            Tags = tags;
            Status = status;
        }
    }
}