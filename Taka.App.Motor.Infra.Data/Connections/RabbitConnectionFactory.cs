using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
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

                return factory.CreateConnection();
            });
        }
    }
}
