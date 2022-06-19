using AdminService.EventHandlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AdminService.Services
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly ILogger<MessageBusSubscriber> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, ILogger<MessageBusSubscriber> logger, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_configuration.GetValue<string>("RabbitMq:Uri"))
            };

            // Create connection
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                // Subscribing to publisher
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

                _connection.ConnectionShutdown += OnConnectionShutdown;

                _logger.LogInformation("RabbitMQ connection established, waiting for message ...");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ connection failed");
            }
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("Disconnected from RabbitMQ");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.ToArray());
                _logger.LogInformation($"Message received: {message}");

                // Handle Message
                _eventProcessor.ProcessEvent(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
