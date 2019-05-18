using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ClientModels.Troubles
{
    public class TroublePatchInfo
    {
        [DataMember(IsRequired = false)]
        public string Name { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Description { get; set; }
        
        [DataMember(IsRequired = false)]
        public IReadOnlyList<string> Images { get; set; }
        
        [DataMember(IsRequired = false)]
        public IReadOnlyList<double> Coordinates { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Address { get; set; }
        
        [DataMember(IsRequired = false)]
        public IReadOnlyList<string> Tags { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Status { get; set; }
    }
}