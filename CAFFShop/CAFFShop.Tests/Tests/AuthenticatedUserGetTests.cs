using CAFFShop.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CAFFShop.Tests.Tests
{
    public class AuthenticatedUserGetTests : IntegrationTestBase
    {
        public AuthenticatedUserGetTests(CaffShopFactory factory) : base(factory)
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
                new KeyValuePair<string, string>("Email", "user@teszt.hu"),
                new KeyValuePair<string, string>("Password", "Password123."),
                new KeyValuePair<string, string>("__RequestVerificationToken", verificationToken),
            });

            var loginResponse = await client.PostAsync("/Identity/Account/Login", contentToSend);
        }

        [Theory]
        [InlineData("/Animations")]
        [InlineData("/Animations/Index")]
        [InlineData("/Animations/Upload")]
        public async Task CanGetPages(string url)
        {
            // Arrange
            using var client = factory.CreateClient();
            await LoginAsync(client);

            // Act
            var response = await client.GetAsync(url);

            // Assert: Check whether the authenticated user gets the pages
            Assert.True(response.IsSuccessStatusCode);
            Assert.EndsWith(url, response.RequestMessage.RequestUri.ToString());
        }

        [Theory]
        [InlineData("/Users")]
        [InlineData("/Users/Index")]
        [InlineData("/Animations/Review")]
        public async Task GetsAccessDenied(string url)
        {
            // Arrange
            using var client = factory.CreateClient();
            await LoginAsync(client);

            // Act
            var response = await client.GetAsync(url);

            // Assert: Check whether the authenticated user gets access denied
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("AccessDenied", response.RequestMessage.RequestUri.ToString());
        }
    }
}
