using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Taka.App.Motor.Domain.Exceptions;
using Taka.App.Motor.Domain.Interfaces;
using Newtonsoft.Json;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Taka.App.Motor.Domain.Events;
using Taka.App.Motor.Domain.Entitites;
using System.Threading;

namespace Taka.App.Motor.Application.BackgroundService
{
    public class RabbitMQConsumerService : IHostedService, IDisposable
    {
        private readonly IRabbitConnectionFactory _connectionFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private CancellationTokenSource? _cancellationTokenSource;
        private IConnection? _connection;
        private IModel? _channel;
        private Timer? _timer;

        public RabbitMQConsumerService(IConfiguration configuration, IRabbitConnectionFactory connectionFactory, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _connectionFactory = connectionFactory;            
            _scopeFactory = scopeFactory;            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("RabbitMQ Consumer Service running.");
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _ = Task.Run(() => ConsumeMessages(_cancellationTokenSource.Token));

            return Task.CompletedTask;
        }


        private async Task ConsumeMessages(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var connection = await _connectionFactory.CreateConnection())
                    using (_channel = connection.CreateModel())
                    {
                        _channel.ExchangeDeclare(exchange: _configuration["RabbitMQSettings:Consumer:Exchange"], type: "direct", durable: true);
                        _channel.QueueDeclare(queue: _configuration["RabbitMQSettings:Consumer:Queue"],
                                   durable: true, exclusive: false, autoDelete: false,
                                   arguments: new Dictionary<string, object>{
                              {"x-dead-letter-exchange", "dead_letter_exchange"}
                                   });

                        _channel.QueueBind(queue: _configuration["RabbitMQSettings:Consumer:Queue"],
                                           exchange: _configuration["RabbitMQSettings:Consumer:Exchange"],
                                           routingKey: _configuration["RabbitMQSettings:Consumer:RoutingKey"]);


                        var consumer = new EventingBasicConsumer(_channel);
                        consumer.Received += async (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var messageFull = $"Received message: {message}";
                            Log.Information(messageFull);

                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var motorcycleService = scope.ServiceProvider.GetRequiredService<IMotorcycleService>();
                                try
                                {
                                    var jsonString = JsonConvert.DeserializeObject<string>(message) ?? throw new AppException("Failed to try to read the message in the queue.");
                                    var response = JsonConvert.DeserializeObject<MotorcycleCreatedEvent>(jsonString);

                                    if (response?.Year == 2024)
                                    {
                                        var motorcycle = new Motorcycle { MotorcycleId = response.MotorcycleId, Year = response.Year, Model = response.Model, Plate = response.Plate };
                                        await motorcycleService.AddConfirmAsync(motorcycle);
                                    }

                                    _channel?.BasicAck(ea.DeliveryTag, false);
                                }
                                catch (DomainException ex)
                                {
                                    Log.Error(ex, "Failed domain validation. Remove message in queue.");
                                    _channel?.BasicNack(ea.DeliveryTag, false, false);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex, "Failed to process message");
                                    _channel?.BasicNack(ea.DeliveryTag, false, true);
                                }
                            }
                        };

                        _channel.BasicConsume(queue: _configuration["RabbitMQSettings:Consumer:Queue"], autoAck: false, consumer: consumer);
                                                
                        await Task.Delay(Timeout.Infinite, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while consuming messages.");
                    await Task.Delay(1000); 
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("RabbitMQ Consumer Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _channel?.Close();
            _connection?.Close();
        }
    }
}