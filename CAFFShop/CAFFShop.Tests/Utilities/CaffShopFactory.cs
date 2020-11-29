using CAFFShop.Api;
using CAFFShop.Dal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace CAFFShop.Tests.Utilities
{
    public class CaffShopFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<CaffShopContext>));

                services.Remove(descriptor);

                var connection = new SqliteConnection("Data Source=:memory:");
                services.AddDbContext<CaffShopContext>(options =>
                {
                    options.UseSqlite(connection);
                });
            });
        }
    }
}
