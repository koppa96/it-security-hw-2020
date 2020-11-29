using CAFFShop.Api;
using CAFFShop.Dal;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAFFShop.Tests.Utilities
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<CaffShopContext>(config =>
                config.UseSqlite(new SqliteConnection("Data Source=:memory:")));
        }
    }
}
