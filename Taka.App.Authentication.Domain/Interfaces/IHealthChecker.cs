namespace Taka.App.Authentication.Domain.Interfaces
{
    public interface IHealthChecker
    {
        Task<HttpResponseMessage> PerformHealthCheck(string url);
    }
}
