using CAFFShop.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CAFFShop.Dal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;

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
