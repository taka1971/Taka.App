using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Taka.App.Motor.Domain.Events;
using Taka.App.Motor.Domain.Interfaces;

namespace Taka.App.Motor.Application.Handlers
{
    public class MotorcycleCreatedEventHandler : INotificationHandler<MotorcycleCreatedEvent>
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IConfiguration _configuration;

        public MotorcycleCreatedEventHandler(IRabbitMQService rabbitMQService, IConfiguration configuration)
        {
            _configuration = configuration; 
            _rabbitMQService = rabbitMQService;
        }
        public async Task Handle(MotorcycleCreatedEvent notification, CancellationToken cancellationToken)
        {
            var message = JsonConvert.SerializeObject(notification);
            var exchange = _configuration[""];
            var routingKey = _configuration[""];
            await _rabbitMQService.Publish(message, exchange, routingKey);
        }
    }

}
