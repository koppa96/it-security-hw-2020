using CAFFShop.Api.Areas.Identity.Pages.Account;
using CAFFShop.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CAFFShop.Tests.Tests
{
    public class AuthenticatedAdminGetTests : IntegrationTestBase
    {
        public AuthenticatedAdminGetTests(CaffShopFactory factory) : base(factory)
        {
        }

        public async Task LoginAsync(HttpClient client)
        {
            var response = await client.GetAsync("/Identity/Account/Login");
            var verificationToken = await response.Content.ReadAsStringAsync();
            if (verificationToken != null && verificationToken.Length > 0)
            {
                verificationToken = verificationToken.Substring(verificationToken.IndexOf("__RequestVerificationToken"));
                verificationToken = verificationToken.Substring(verificationToken.IndexOf("value=\"") + 7);
                verificationToken = verificationToken.Substring(0, verificationToken.IndexOf("\""));
            }

            var contentToSend = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Email", "admin@teszt.hu"),
                new KeyValuePair<string, string>("Password", "Alma123."),
                new KeyValuePair<string, string>("__RequestVerificationToken", verificationToken),
            });

            var loginResponse = await client.PostAsync("/Identity/Account/Login", contentToSend);
        }

        [Theory]
        [InlineData("/Animations")]
        [InlineData("/Animations/Index")]
        [InlineData("/Animations/Upload")]
        [InlineData("/Users")]
        [InlineData("/Users/Index")]
        [InlineData("/Animations/Review")]
        public async Task CanGetAllPages(string url)
        {
            // Arrange
            using var client = factory.CreateClient();
            await LoginAsync(client);

            // Act
            var response = await client.GetAsync(url);

            // Assert: Check whether the authenticated admin gets the pages
            Assert.True(response.IsSuccessStatusCode);
            Assert.EndsWith(url, response.RequestMessage.RequestUri.ToString());
        }
    }
}
