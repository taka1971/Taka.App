using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Taka.App.Authentication.Domain.Interfaces;

namespace Taka.App.Authentication.Application.BackgroundService
{
    public class HealthCheckBackgroundService : IHostedService, IDisposable
    {
        private readonly IHealthChecker _healthChecker;
        private readonly string _healthCheckUrl;
        private Timer ?_timer;

        public HealthCheckBackgroundService(IHealthChecker healthChecker, IConfiguration configuration)
        {
            _healthChecker = healthChecker;          
            
            _healthCheckUrl = configuration["HealthCheckSettings:Url"] ?? throw new Exception("HealthCheck settings not found.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Health Check Service running.");

            _timer = new Timer(async _ => await DoWork(), null, TimeSpan.Zero,
                TimeSpan.FromSeconds(300));

            return Task.CompletedTask;
        }

        private async Task DoWork()
        {            
            try
            {                
                var response = await _healthChecker.PerformHealthCheck(_healthCheckUrl);
                if (response.IsSuccessStatusCode)
                {
                    Log.Information("Health check passed at: {Time}", DateTimeOffset.Now);
                }
                else
                {
                    Log.Warning("Health check failed at: {Time}", DateTimeOffset.Now);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Health check encountered an exception.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Health Check Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
            }
        }       

    }
}
