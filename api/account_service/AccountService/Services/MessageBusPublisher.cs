using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AccountService.Dtos;
using RabbitMQ.Client;

namespace AccountService.Services
{
    public interface IMessageBusPublisher
    {
        void PublishCreateBoard(MessageCreateBoardPublishDto messageCreateBoardPublishDto);
        void PublishAddSignIn();

    }
    public class MessageBusPublisher : IMessageBusPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusPublisher> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public MessageBusPublisher(IConfiguration configuration, ILogger<MessageBusPublisher> logger)
        {
            _configuration = configuration;
            _logger = logger;


            // connection config object
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

                _connection.ConnectionShutdown += OnConnectionShutdown;

                _logger.LogInformation("RabbitMQ connection established");
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

        public void Dipose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        public void PublishCreateBoard(MessageCreateBoardPublishDto messageCreateBoardPublishDto)
        {
            // publish message create default board for new signin 
            var message = JsonSerializer.Serialize(messageCreateBoardPublishDto);

            _channel.BasicPublish(exchange: "trigger", routingKey: "", body: Encoding.UTF8.GetBytes(message));

            _logger.LogInformation("Message create board sent to RabbitMQ");
        }

        public void PublishAddSignIn()
        {
            // publish message increase signin count
            var message = JsonSerializer.Serialize(new MessageAddSiginPublishDto { Event="AddSignIn"});

            _channel.BasicPublish(exchange: "trigger", routingKey: "", body: Encoding.UTF8.GetBytes(message));

            _logger.LogInformation("Message add SignIn sent to RabbitMQ");
        }
    }
}