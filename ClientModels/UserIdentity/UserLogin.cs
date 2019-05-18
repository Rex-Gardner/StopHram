using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientModels.UserIdentity
{
    [DataContract]
    public class UserLogin
    {
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        [DataMember(IsRequired = true)]
        public string Password { get; set; }

        [DataMember(IsRequired = true)]
        public bool RememberMe { get; set; }
    }
}
