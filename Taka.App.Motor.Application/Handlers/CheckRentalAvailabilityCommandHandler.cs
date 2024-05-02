using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Taka.App.Motor.Domain.Commands;
using Taka.App.Motor.Domain.Interfaces;

namespace Taka.App.Motor.Application.Handlers
{
    public class CheckRentalAvailabilityCommandHandler : IRequestHandler<CheckRentalAvailabilityCommand, bool>
    {
        private readonly IRabbitConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;

        public CheckRentalAvailabilityCommandHandler(IRabbitConnectionFactory connectionFactory, IConfiguration configuration)
        {
            _connectionFactory = connectionFactory;
            _configuration = configuration;
        }

        public async Task<bool> Handle(CheckRentalAvailabilityCommand command, CancellationToken cancellationToken)
        {
            using (var connection = await _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var message = new { MotorcycleId = command.MotorcycleId, QueryDate = DateTime.Now };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(
                    exchange: _configuration["RabbitMQSettings:Producer:Exchange"],
                    routingKey: _configuration["RabbitMQSettings:Producer:RoutingKey"],
                    basicProperties: null,
                    body: body);
            }
            return true;
        }
    }

}
