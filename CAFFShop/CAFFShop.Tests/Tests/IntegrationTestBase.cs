using CAFFShop.Dal;
using CAFFShop.Dal.Constants;
using CAFFShop.Dal.Entities;
using CAFFShop.Tests.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CAFFShop.Tests.Tests
{
    public class IntegrationTestBase : IAsyncLifetime, IClassFixture<CaffShopFactory>
    {
        protected readonly CaffShopFactory factory;

        public IntegrationTestBase(CaffShopFactory factory)
        {
            this.factory = factory;
        }

        public Task DisposeAsync()
        {
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CaffShopContext>();

            return context.Database.CloseConnectionAsync();
        }

        public async Task InitializeAsync()
        {
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CaffShopContext>();
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = RoleTypes.Admin
            });

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var admin = new User
            {
                UserName = "admin@teszt.hu",
                Email = "admin@teszt.hu"
            };
            await userManager.CreateAsync(admin, "Alma123.");
            await userManager.AddToRoleAsync(admin, RoleTypes.Admin);
            await userManager.CreateAsync(new User
            {
                UserName = "user@teszt.hu",
                Email = "user@teszt.hu"
            }, "Password123.");
        }
    }
}
