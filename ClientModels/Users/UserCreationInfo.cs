using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace ClientModels.Users
{
    [DataContract]
    public class UserCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        [DataMember(IsRequired = true)]
        [EmailAddress]
        public string Email { get; set; }

        [DataMember(IsRequired = true)]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataMember(IsRequired = true)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(IsRequired = true)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
