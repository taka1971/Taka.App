using Polly;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Taka.App.Motor.Domain.Interfaces;

namespace Taka.App.Motor.Infra.Data.Connections
{
    public class RabbitConnectionFactory : IRabbitConnectionFactory
    {
        private readonly IConfiguration _configuration;        

        public RabbitConnectionFactory(IConfiguration configuration)
        {            
            _configuration = configuration;
        }

        public async Task<IConnection> CreateConnection()
        {
            return await Task.Run(() =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration["RabbitMQSettings:HostName"],
                    UserName = _configuration["RabbitMQSettings:UserName"],
                    Password = _configuration["RabbitMQSettings:Password"]
                };

                return Policy
             .Handle<Exception>()
             .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
             .Execute(() => factory.CreateConnection());

            });
        }                
    }
}
