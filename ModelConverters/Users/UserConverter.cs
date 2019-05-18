using System;
using System.Collections.Generic;
using System.Text;
using Client = ClientModels.Users;
using Model = Models.Users;

namespace ModelConverters.Users
{
    public static class UserConverter
    {
        public static Client.User Convert(Model.User modelUser)
        {
            if (modelUser == null)
            {
                throw new ArgumentNullException(nameof(modelUser));
            }

            var clientUser = new Client.User
            {
                Id = modelUser.Id,
                UserName = modelUser.NormalizedUserName,
                Email = modelUser.Email,
                PhoneNumber = modelUser.PhoneNumber,
                Roles = modelUser.Roles,
                CreatedTroubles = (IReadOnlyList<string>)modelUser.CreatedTroubles,
                LikedTroubles = (IReadOnlyList<string>)modelUser.LikedTroubles,
                CreatedAt = modelUser.CreatedAt,
                LastUpdatedAt = modelUser.LastUpdatedAt
            };

            return clientUser;
        }
    }
}
