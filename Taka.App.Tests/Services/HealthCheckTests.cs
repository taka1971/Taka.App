using NSubstitute;
using System.Net;
using Taka.App.Authentication.Infra.Services.HealthCheck;

namespace Taka.App.Authentications.Tests.Validations
{
    public class HealthCheckerTests
    {
        private readonly HealthChecker _healthChecker;
        private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
        private HttpClient _client;
        private HttpResponseMessage _responseToReturn;

        public HealthCheckerTests()
        {
            var handler = new TestHttpMessageHandler(() => _responseToReturn);
            _client = new HttpClient(handler);
            _httpClientFactory.CreateClient().Returns(_client);
            _healthChecker = new HealthChecker(_httpClientFactory);
        }

        [Fact]
        public async Task PerformHealthCheck_ShouldReturnSuccess_WhenUrlIsValid()
        {
            // Arrange
            var expectedUrl = "http://validurl.com";
            _responseToReturn = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            var response = await _healthChecker.PerformHealthCheck(expectedUrl);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PerformHealthCheck_ShouldHandleHttpClientExceptions()
        {
            // Arrange
            var invalidUrl = "http://invalidurl.com";
            _responseToReturn = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };

            // Act
            var response = await _healthChecker.PerformHealthCheck(invalidUrl);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task PerformHealthCheck_ShouldReturnCorrectStatusCode_WhenServerRespondsWithError(HttpStatusCode statusCode)
        {
            // Arrange
            var errorUrl = "http://errorurl.com";
            _responseToReturn = new HttpResponseMessage(statusCode);

            // Act
            var response = await _healthChecker.PerformHealthCheck(errorUrl);

            // Assert
            Assert.Equal(statusCode, response.StatusCode);
        }

        private class TestHttpMessageHandler : DelegatingHandler
        {
            private readonly Func<HttpResponseMessage> _responseGenerator;

            public TestHttpMessageHandler(Func<HttpResponseMessage> responseGenerator)
            {
                _responseGenerator = responseGenerator;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_responseGenerator());
            }
        }
    }
}