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
        // Test sending data to the left endpoint
        public async Task TestPutLeftData()
        {
            // Arrange
            var request = new
            {
                Url = "v1/diff/1/left",
                Body = new
                {
                    data = "AAAAAA=="
                }
            };

            // Act
            StringContent content = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PutAsync(request.Url, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        // Test sending data to the right endpoint
        public async Task TestPutRightData()
        {
            // Arrange
            var request = new
            {
                Url = "v1/diff/1/right",
                Body = new
                {
                    data = "AQABAQ=="
                }
            };

            // Act
            StringContent content = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PutAsync(request.Url, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        // Test getting data by given id
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
        // There should not be any data with id 3 in a database for this test to work properly
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
        // We test get method for a case where there is only data at the left endpoint for an id 2
        public async Task TestGetDiffWithLeftData()
        {
            // Arrange
            var requestUrl = "v1/diff/2";
            string expected = "NotFound";
            // We put a data to the left endpoint with id 2 for a case it wasn't created before
            var requestLeft = new
            {
                Url = "v1/diff/2/left",
                Body = new
                {
                    data = "AQABAQ=="
                }
            };
            StringContent leftContent = new StringContent(JsonConvert.SerializeObject(requestLeft.Body), Encoding.Default, "application/json");
            var leftResponse = await _client.PutAsync(requestLeft.Url, leftContent);

            // Act
            var response = await _client.GetAsync(requestUrl);
            string status = response.StatusCode.ToString();

            // Assert
            Assert.Equal(status, expected);
        }
    }
}
