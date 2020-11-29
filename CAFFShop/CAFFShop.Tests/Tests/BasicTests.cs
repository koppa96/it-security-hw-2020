using CAFFShop.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CAFFShop.Tests.Tests
{
    public class BasicTests : IntegrationTestBase
    {
        public BasicTests(CaffShopFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CanGetAnimationList()
        {
            using var client = factory.CreateClient();
            var response = await client.GetAsync("Animations/Index");

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
