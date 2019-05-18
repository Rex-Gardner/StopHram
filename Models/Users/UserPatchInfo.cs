using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Users
{
    public class UserPatchInfo
    {
        public string UserName { get; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public IReadOnlyList<Guid> CreatedTroubles { get; set; }
        public IReadOnlyList<Guid> LikedTroubles { get; set; }

        public UserPatchInfo(string userName, string oldPassword = null, string password = null,
            IReadOnlyList<Guid> createdTroubles = null, IReadOnlyList<Guid> likedTroubles = null)
        {
            UserName = userName;
            OldPassword = oldPassword;
            Password = Password;
            CreatedTroubles = createdTroubles;
            LikedTroubles = likedTroubles;
        }
    }
}
