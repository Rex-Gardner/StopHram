using System;
using System.Collections.Generic;
using System.Text;
using Model = Models.Roles;
using Client = ClientModels.Roles;

namespace ModelConverters.Roles
{
    public static class RoleConverter
    {
        public static Client.Role Convert(Model.Role modelRole)
        {
            if (modelRole == null)
            {
                throw new ArgumentNullException(nameof(modelRole));
            }

            var clientRole = new Client.Role
            {
                Id = modelRole.Id,
                Name = modelRole.Name,
            };

            return clientRole;
        }
    }
}
