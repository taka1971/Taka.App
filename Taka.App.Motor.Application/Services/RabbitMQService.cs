using Newtonsoft.Json;
using RabbitMQ.Client;
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


                if (channel.WaitForConfirms())
                {
                    Console.WriteLine("Mensagem publicada com sucesso.");
                }
                else
                {
                    Console.WriteLine("Falha ao publicar a mensagem.");
                }
            }
        }
    }
}
