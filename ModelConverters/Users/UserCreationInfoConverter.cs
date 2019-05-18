using System;
using System.Collections.Generic;
using System.Text;
using Client = ClientModels.Users;
using Model = Models.Users;

namespace ModelConverters.Users
{
    public static class UserCreationInfoConverter
    {
        public static Model.UserCreationInfo Convert(Client.UserCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.UserName))
            {
                throw new ArgumentException(nameof(clientCreationInfo.UserName));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.Email))
            {
                throw new ArgumentNullException(nameof(clientCreationInfo.Email));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.PhoneNumber))
            {
                throw new ArgumentException(nameof(clientCreationInfo.PhoneNumber));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.Password))
            {
                throw new ArgumentException(nameof(clientCreationInfo.Password));
            }

            var modelCreationInfo = new Model.UserCreationInfo(
                clientCreationInfo.UserName,
                clientCreationInfo.Email,
                clientCreationInfo.PhoneNumber,
                clientCreationInfo.Password
            );

            return modelCreationInfo;
        }
    }
}
