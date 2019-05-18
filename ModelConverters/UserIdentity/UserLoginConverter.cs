using System;
using System.Collections.Generic;
using System.Text;
using Model = Models.UserIdentity;
using Client = ClientModels.UserIdentity;

namespace ModelConverters.UserIdentity
{
    public static class UserLoginConverter
    {
        public static Model.UserLogin Convert(Client.UserLogin clientUserLogin)
        {
            if (clientUserLogin == null)
            {
                throw new ArgumentNullException(nameof(clientUserLogin));
            }

            if (string.IsNullOrEmpty(clientUserLogin.UserName))
            {
                throw new ArgumentException(nameof(clientUserLogin.UserName));
            }

            if (string.IsNullOrEmpty(clientUserLogin.Password))
            {
                throw new ArgumentException(nameof(clientUserLogin.Password));
            }

            var modelUserLogin = new Model.UserLogin 
                (clientUserLogin.UserName,
                clientUserLogin.Password,
                clientUserLogin.RememberMe);
            return modelUserLogin;
        }
    }
}
