
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Taka.App.Rentals.Domain.Commands;


namespace Taka.App.Rentals.Application.Handlers
{
    public class CheckRentalAvailabilityHandler : IRequestHandler<CheckRentalAllowedCommand>
    {
        private readonly IConfiguration _configuration;
        public CheckRentalAvailabilityHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Handle(CheckRentalAllowedCommand command, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQSettings:HostName"], UserName= _configuration["RabbitMQSettings:UserName"], Password= _configuration["RabbitMQSettings:Password"] };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var message = new { MotorcycleId = command.MotorcycleId, Permited = command.Permited };
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                    channel.BasicPublish(exchange: _configuration["RabbitMQSettings:Producer:Exchange"],
                                         routingKey: _configuration["RabbitMQSettings:Producer:RoutingKey"],
                                         basicProperties: null,
                                         body: body);
                }
            });
        }
    }
}
