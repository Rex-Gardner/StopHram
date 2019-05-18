using System.Runtime.Serialization;

namespace ClientModels.Tags
{
    public class TagPatchInfo
    {
        [DataMember(IsRequired = false)]
        public string Name { get; set; }
    }
}