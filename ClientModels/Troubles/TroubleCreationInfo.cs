using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ClientModels.Troubles
{
    public class TroubleCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
        
        [DataMember(IsRequired = true)]
        public IReadOnlyList<double> Coordinates { get; set; }
        
        [DataMember(IsRequired = true)]
        public string Address { get; set; }
        
        [DataMember(IsRequired = false)]
        public IReadOnlyList<string> Tags { get; set; }
    }
}