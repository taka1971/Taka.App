using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text;
using Newtonsoft.Json;
using Taka.App.Rentals.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Taka.App.Rentals.Domain.Exceptions;
using Taka.App.Rentals.Domain.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Serilog;


namespace Taka.App.Rentals.Application.BackgroundService
{
    public class RabbitMQConsumerService : IHostedService, IDisposable
    {

        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IModel? _channel;
        private Timer? _timer;

        public RabbitMQConsumerService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration["RabbitMQSettings:HostName"],
                    UserName = _configuration["RabbitMQSettings:UserName"],
                    Password = _configuration["RabbitMQSettings:Password"]
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: _configuration["RabbitMQSettings:Consumer:Exchange"], type: "direct", durable: true);
                _channel.QueueDeclare(queue: _configuration["RabbitMQSettings:Consumer:Queue"],
                       durable: true, exclusive: false, autoDelete: false,
                       arguments: new Dictionary<string, object>{
                          {"x-dead-letter-exchange", "dead_letter_exchange"}
                       });

                _channel.QueueBind(queue: _configuration["RabbitMQSettings:Consumer:Queue"],
                                   exchange: _configuration["RabbitMQSettings:Consumer:Exchange"],
                                   routingKey: _configuration["RabbitMQSettings:Consumer:RoutingKey"]);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("RabbitMQ Consumer Service running.");

            _timer = new Timer(ConsumeMessages, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(3));

            return Task.CompletedTask;
        }

        private void ConsumeMessages(object? state)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var messageFull = $"Received message: {message}";
                Log.Information(messageFull);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var rentalService = scope.ServiceProvider.GetRequiredService<IRentalService>();
                    try
                    {
                        var request = JsonConvert.DeserializeObject<RentalCheckRequest>(message) ?? throw new AppException("Failed to try to read the message in the queue.");

                        await rentalService.ResponseCheckRentalByMotorcycleIdAsync(request.MotorcycleId);

                        _channel?.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Failed to process message");
                        _channel?.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
            };

            _channel.BasicConsume(queue: _configuration["RabbitMQSettings:Consumer:Queue"], autoAck: false, consumer: consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("RabbitMQ Consumer Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
                _channel?.Close();
                _connection?.Close();
            }
        }
    }
}