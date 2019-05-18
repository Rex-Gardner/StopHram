using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace Models.Troubles
{
    public class TroubleCreationInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<string> Images { get; set; }
        public GeoCoordinate Coordinates { get; set; }
        public string Address { get; set; }
        public IReadOnlyList<string> Tags { get; set; }

        public TroubleCreationInfo(string name, string description, IEnumerable<string> images, 
            double latitude, double longitude, string address, IEnumerable<string> tags)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Images = images?.ToArray() ?? new string[0];
            Coordinates = new GeoCoordinate(latitude, longitude);
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Tags = tags?.ToArray() ?? new string[0];
        }
    }
}