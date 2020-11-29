using CAFFShop.Dal;
using CAFFShop.Dal.Constants;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace CAFFShop.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CaffShopContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                
                await context.Database.MigrateAsync();
                if (!(await userManager.Users.AnyAsync()))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>
                    {
                        Name = RoleTypes.Admin
                    });

                    var user = new User
                    {
                        UserName = configuration.GetValue<string>("DefaultAdministrator:Email"),
                        Email = configuration.GetValue<string>("DefaultAdministrator:Email")
                    };

                    await userManager.CreateAsync(user, configuration.GetValue<string>("DefaultAdministrator:Password"));
                    await userManager.AddToRoleAsync(user, RoleTypes.Admin);
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, loggerConfig) =>
                {
                    loggerConfig.ReadFrom.Configuration(context.Configuration, "Serilog");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
