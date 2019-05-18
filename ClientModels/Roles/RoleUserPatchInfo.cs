using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientModels.Roles
{
    [DataContract]
    public class RoleUserPatchInfo
    {
        [DataMember(IsRequired = true)]
        public IReadOnlyList<string> UserRoles { get; set; }
    }
}
