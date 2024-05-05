using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using Taka.App.Motor.Domain.Interfaces;

namespace Taka.App.Motor.Application.Services
{
    public class RabbitMQService: IRabbitMQService
    {
        private readonly IRabbitConnectionFactory _connectionFactory;        
        public RabbitMQService(IRabbitConnectionFactory connectionFactory) =>  _connectionFactory = connectionFactory;            
        

        public async Task Publish(string message, string exchange, string routingKey)
        {
            var retryPolicy = Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            await retryPolicy.ExecuteAsync(async () =>
            {
                using (var connection = await _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.ConfirmSelect();
                    channel.BasicPublish(
                    exchange: exchange,
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: body);
                }
            });
        }
    }
}
