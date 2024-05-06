using RabbitMQ.Client;

namespace Taka.App.Motor.Domain.Interfaces
{
    public interface IRabbitConnectionFactory
    {
        Task<IConnection> CreateConnection();
    }
}
