using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DiffIntegrationTests
{
    public class DiffIntegrationTest : IClassFixture<WebApplicationFactory<DiffingApi.Startup>>
    {
        private readonly WebApplicationFactory<DiffingApi.Startup> _factory;
        private readonly HttpClient _client;

        public DiffIntegrationTest(WebApplicationFactory<DiffingApi.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task TestPutLeftData()
        {
            // Arrange
            var request = new
            {
                Url = "v1/diff/1/left",
                Body = new
                {
                    data = "AAAAAA"
                }
            };

            // Act
            StringContent content = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PutAsync(request.Url, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestPutRightData()
        {
            // Arrange
            var request = new
            {
                Url = "v1/diff/1/right",
                Body = new
                {
                    data = "AABBAAC"
                }
            };

            // Act
            StringContent content = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PutAsync(request.Url, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        // For this test to work properly there already must be data at the left and right endpoints with id 1 before testing
        public async Task TestGetDiff()
        {
            // Arrange
            var requestUrl = "v1/diff/1";

            // Act
            var response = await _client.GetAsync(requestUrl);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestGetEmptyDiff()
        {
            // Arrange
            var requestUrl = "v1/diff/3";
            string expected = "NotFound";

            // Act
            var response = await _client.GetAsync(requestUrl);
            string status = response.StatusCode.ToString();

            // Assert
            Assert.Equal(status, expected);
        }

        [Fact]
        // For this test to work properly there already must be data at the left endpoint
        // with id 2 before testing (right endpoint data must be empty)
        public async Task TestGetDiffWithLeftData()
        {
            // Arrange
            var requestUrl = "v1/diff/2";
            string expected = "NotFound";

            // Act
            var response = await _client.GetAsync(requestUrl);
            string status = response.StatusCode.ToString();

            // Assert
            Assert.Equal(status, expected);
        }
    }
}
