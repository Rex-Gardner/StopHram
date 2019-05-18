using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models.Roles;
using Models.Users;

namespace Models.UserIdentity
{
    public static class Initializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            string userName = "admin";
            string email = "admin@urban.ru";
            string password = "_Aa12345";
            string phoneNumber = "8800-000-00-00";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new Role("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new Role("user"));
            }

            var dateTime = DateTime.UtcNow;

            if (await userManager.FindByNameAsync(userName) == null)
            {
                User admin = new User
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    CreatedAt = dateTime,
                    LastUpdatedAt = dateTime

                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

        }
    }
}
