using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Taka.App.Motor.Domain.Events;
using Taka.App.Motor.Domain.Interfaces;

namespace Taka.App.Motor.Application.Handlers
{
    public class ResultCreatedEventHandler : INotificationHandler<ResultMotorcycleCreatedEvent>
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IConfiguration _configuration;

        public ResultCreatedEventHandler(IRabbitMQService rabbitMQService, IConfiguration configuration)
        {
            _configuration = configuration;
            _rabbitMQService = rabbitMQService;
        }
        public async Task Handle(ResultMotorcycleCreatedEvent notification, CancellationToken cancellationToken)
        {
            var message = JsonConvert.SerializeObject(notification);
            var exchange = _configuration["RabbitMQSettings:Producer:Exchange"];
            var routingKey = _configuration["RabbitMQSettings:Producer:ResultRoutingKey"];
            await _rabbitMQService.Publish(message, exchange, routingKey);
        }
    }
}
