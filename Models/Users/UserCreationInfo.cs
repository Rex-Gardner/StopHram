using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Users
{
    public class UserCreationInfo
    {
        public string UserName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string Password { get; }

        public UserCreationInfo(string userName, string email, string phoneNumber, string password)
        {
            UserName = userName?.ToLower() ?? throw new ArgumentNullException(nameof(userName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}
