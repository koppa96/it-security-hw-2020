using CAFFShop.Tests.Utilities;
using System.Threading.Tasks;
using Xunit;

namespace CAFFShop.Tests.Tests
{
    public class UnauthenticatedUserGetTests : IntegrationTestBase
    {
        public UnauthenticatedUserGetTests(CaffShopFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("/Animations")]
        [InlineData("/Animations/Index")]
        [InlineData("/Identity/Account/Login")]
        [InlineData("/Identity/Account/Register")]
        public async Task CanGetPageWithoutLogin(string url)
        {
            // Arrange
            using var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert: Check whether the unauthenticated user gets the pages
            Assert.True(response.IsSuccessStatusCode);
            Assert.EndsWith(url, response.RequestMessage.RequestUri.ToString());
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        public async Task RedirectedToAnimationList(string url)
        {
            // Arrange
            using var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert: Check whether the user is redirected to the animation list page
            Assert.True(response.IsSuccessStatusCode);
            Assert.EndsWith("/Animations", response.RequestMessage.RequestUri.ToString());
        }

        [Theory]
        [InlineData("/Animations/Upload")]
        [InlineData("/Users")]
        [InlineData("/Users/Index")]
        [InlineData("/Animations/Review")]
        public async Task CanNotViewProtectedPagesWithoutLogin(string url)
        {
            // Arrange
            using var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert: Check whether the unauthenticated user is redirected
            Assert.Contains("/Identity/Account/Login", response.RequestMessage.RequestUri.ToString());
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
