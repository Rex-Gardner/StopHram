using System;
using System.Collections.Generic;
using System.Text;

namespace Models.UserIdentity
{
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public UserLogin(string userName, string password, bool rememberMe)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            RememberMe = rememberMe;
        }
    }
}
