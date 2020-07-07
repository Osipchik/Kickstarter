using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Company;
using CompanyTests.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CompanyTests.IntegrationTests
{
    public class PreviewTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        public PreviewTest()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task UserPreviewsAdmin()
        {
            var client = MockAuthClient.GetClient(_factory);

            var httpResponse = await client.GetAsync("api/v1/Preview/GetUserCompanies?take=10&skip=0&userId=101");
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        [Fact]
        public async Task UserPreviewsUnauthorized()
        {
            var client = MockAuthClient.GetClient(_factory);

            var httpResponse = await client.GetAsync("api/v1/Preview/GetUserCompanies?take=10&skip=0&userId=101");
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }
    }
}