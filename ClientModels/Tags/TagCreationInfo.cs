
using System.Runtime.Serialization;

namespace ClientModels.Tags
{
    public class TagCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}