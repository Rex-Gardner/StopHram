using System;
using System.Collections.Generic;
using System.Text;
using Model = Models.Roles;
using Client = ClientModels.Roles;

namespace ModelConverters.Roles
{
    public static class RoleUserPatchInfoConverter
    {
        public static Model.RoleUserPatchInfo Converter(Client.RoleUserPatchInfo clientRoleUserPatchInfo)
        {
            if (clientRoleUserPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientRoleUserPatchInfo));
            }

            var modelRoleUserPatchInfo = new Model.RoleUserPatchInfo(clientRoleUserPatchInfo.UserName, clientRoleUserPatchInfo.UserRoles);

            return modelRoleUserPatchInfo;
        }
    }
}
