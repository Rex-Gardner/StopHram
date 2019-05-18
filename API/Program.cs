using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.Roles;
using Models.UserIdentity;
using Models.Users;
using System;
using System.Threading.Tasks;

namespace API
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                Task taskInit;
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<Role>>();
                    taskInit = Initializer.InitializeAsync(userManager, rolesManager);
                    taskInit.Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();

        }
    }
}