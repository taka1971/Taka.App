using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Taka.App.Motor.Domain.Commands;

namespace Taka.App.Motor.Application.Handlers
{
    public class CheckRentalAvailabilityHandler : IRequestHandler<CheckRentalAvailabilityCommand, bool>
    {
        private readonly IConfiguration _configuration;
        public CheckRentalAvailabilityHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Handle(CheckRentalAvailabilityCommand command, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQSettings:HostName"],
                UserName = _configuration["RabbitMQSettings:UserName"],  
                Password = _configuration["RabbitMQSettings:Password"]   
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var message = new { MotorcycleId = command.MotorcycleId, QueryDate = DateTime.Now };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchange: _configuration["RabbitMQSettings:Producer:Exchange"],
                                     routingKey: _configuration["RabbitMQSettings:Producer:RoutingKey"],
                                     basicProperties: null,
                                     body: body);
            }
            return await Task.FromResult(true); 
        }
    }
}
