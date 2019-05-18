using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace ClientModels.Users
{
    [DataContract]
    public class UserPatchInfo
    {
        [DataMember(IsRequired = false)]
        [DataType((DataType.Password))]
        public string OldPassword { get; set; }

        [DataMember(IsRequired = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(IsRequired = false)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataMember(IsRequired = false)]
        public IReadOnlyList<Guid> CreatedTroubles { get; set; }

        [DataMember(IsRequired = false)]
        public IReadOnlyList<Guid> LikedTroubles { get; set; }

    }
}
