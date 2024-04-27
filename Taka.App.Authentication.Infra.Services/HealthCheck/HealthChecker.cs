using Taka.App.Authentication.Domain.Interfaces;

namespace Taka.App.Authentication.Infra.Services.HealthCheck
{
    public class HealthChecker : IHealthChecker
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HealthChecker(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> PerformHealthCheck(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            
            return response;
        }
    }
}
