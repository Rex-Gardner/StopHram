using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Roles
{
    public class RoleUserPatchInfo
    {
        public string UserName { get; }
        public IReadOnlyList<string> UserRoles { get; set; }

        public RoleUserPatchInfo(string userName, IReadOnlyList<string> userRoles = null)
        {
            UserName = userName;
            UserRoles = userRoles;
        }
    }
}
