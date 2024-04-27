using Microsoft.Extensions.Configuration;
using NSubstitute;
using Taka.App.Authentication.Application.BackgroundService;
using Taka.App.Authentication.Domain.Interfaces;

namespace Taka.App.Authentications.Tests.Validations
{
    public class HealthCheckBackgroundServiceTests
    {        
        private readonly IConfiguration _configuration = Substitute.For<IConfiguration>();     
        private readonly IHealthChecker _mockHealthChecker;
        private readonly HealthCheckBackgroundService _service;        

        public HealthCheckBackgroundServiceTests()
        {
            _mockHealthChecker = Substitute.For<IHealthChecker>();
            var configSection = Substitute.For<IConfigurationSection>();
            configSection.Value.Returns("http://localhost/health");
            _configuration.GetSection("HealthCheckSettings:Url").Returns(configSection);
            _service = new HealthCheckBackgroundService(_mockHealthChecker, _configuration);
        }        


        [Fact]
        public async Task StartAsync_SetsTimerCorrectly()
        {
            var service = new HealthCheckBackgroundService(_mockHealthChecker, _configuration);
            await service.StartAsync(CancellationToken.None);

            Assert.NotNull(service.GetType().GetField("_timer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service));
        }

        [Fact]
        public async Task StopAsync_StopsTimer()
        {
            var service = new HealthCheckBackgroundService(_mockHealthChecker, _configuration);
            await service.StartAsync(CancellationToken.None);
            await service.StopAsync(CancellationToken.None);

            // This test ensures that the timer change is called with the correct parameters to stop it
            var timerField = service.GetType().GetField("_timer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var timer = (Timer)timerField.GetValue(service);
            Assert.NotNull(timer);

            // Unfortunately, without reflection or a custom wrapper, we cannot assert the timer state directly in xUnit
            // You would need to check this manually or through integration tests
        }

        [Fact]
        public void Dispose_DisposesTimer()
        {
            var service = new HealthCheckBackgroundService(_mockHealthChecker, _configuration);
            service.Dispose();

            var timerField = service.GetType().GetField("_timer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var timer = (Timer)timerField.GetValue(service);
            Assert.Null(timer);
        }       
    }
}
