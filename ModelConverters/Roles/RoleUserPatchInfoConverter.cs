using System;
using System.Collections.Generic;
using System.Text;
using Model = Models.Roles;
using Client = ClientModels.Roles;

namespace ModelConverters.Roles
{
    public static class RoleUserPatchInfoConverter
    {
        public static Model.RoleUserPatchInfo Converter(string userName, Client.RoleUserPatchInfo clientRoleUserPatchInfo)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (clientRoleUserPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientRoleUserPatchInfo));
            }

            var modelRoleUserPatchInfo = new Model.RoleUserPatchInfo(userName, clientRoleUserPatchInfo.UserRoles);

            return modelRoleUserPatchInfo;
        }
    }
}
