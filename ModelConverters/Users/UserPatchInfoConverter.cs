using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client = ClientModels.Users;
using Model = Models.Users;

namespace ModelConverters.Users
{
    public static class UserPatchInfoConverter
    {
        public static Model.UserPatchInfo Convert(string userName, Client.UserPatchInfo clientPatchInfo)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            var modelPatchInfo = new Model.UserPatchInfo(userName, clientPatchInfo.OldPassword,
                clientPatchInfo.Password);
            return modelPatchInfo;
        }
    }
}
