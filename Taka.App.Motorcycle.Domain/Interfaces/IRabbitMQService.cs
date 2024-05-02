namespace Taka.App.Motor.Domain.Interfaces
{
    public interface IRabbitMQService
    {
        Task Publish(string message, string exchange, string routingKey);
    }
}
