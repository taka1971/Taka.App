using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using Taka.App.Motor.Domain.Interfaces;

public class RabbitMqInit
{
    private readonly IConfiguration _configuration;
    private readonly IRabbitConnectionFactory _connectionFactory;

    public RabbitMqInit(IConfiguration configuration, IRabbitConnectionFactory connectionFactory)
    {
        _configuration = configuration;
        _connectionFactory = connectionFactory;
    }

    public void Initialize()
    {        
        EnsureExchangesAndBindingsExist();
    }

    private void EnsureExchangesAndBindingsExist()
    {
        string exchangeName = _configuration["RabbitMQSettings:Producer:Exchange"];
        
        string routingKey = _configuration["RabbitMQSettings:Producer:RoutingKey"];
        string queueName = _configuration["RabbitMQSettings:Queue"];

        string resultRoutingKey = _configuration["RabbitMQSettings:Producer:ResultRoutingKey"];
        string resultQueueName = _configuration["RabbitMQSettings:ResultQueue"];

        using (var connection = _connectionFactory.CreateConnection().Result)
        using (var channel = connection.CreateModel())
        {         
            if (!ExchangeExists(channel, exchangeName))
            {         
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);
                Log.Information($"Exchange '{exchangeName}' created.");
            }
            
            if (!QueueExists(channel, queueName))
            {             
                channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queueName, exchangeName, routingKey);
                Log.Information($"Queue '{queueName}' created.");
            }

            if (!QueueExists(channel, resultQueueName))
            { 
                channel.QueueBind(resultQueueName, exchangeName, resultRoutingKey);
                Log.Information($"Queue '{resultQueueName}' created.");
            }
        }
    }

    private bool ExchangeExists(IModel channel, string exchangeName)
    {
        try
        {            
            channel.ExchangeDeclarePassive(exchangeName);
            return true;
        }
        catch (Exception)
        {         
            return false;
        }
    }

    private bool QueueExists(IModel channel, string queueName)
    {
        try
        {         
            channel.QueueDeclarePassive(queueName);
            return true;
        }
        catch (Exception)
        {         
            return false;
        }
    }

    
}
