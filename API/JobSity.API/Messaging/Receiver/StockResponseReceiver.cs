using JobSity.API.Hubs;
using JobSity.API.ViewModels;
using JobSity.Model.Models.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace JobSity.Messaging.Receiver
{
    public class StockResponseReceiver : BackgroundService
    {
        private readonly ChatHubService<ChatHub> _hubMethods;
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public StockResponseReceiver(ChatHubService<ChatHub> hubMethods, 
                                     IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _port = rabbitMqOptions.Value.Port;
            _hubMethods = hubMethods;
            _queueName = "StockResponse";

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener(int retryTimes = 0)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                Port = _port
                //UserName = _username,
                //Password = _password
            };

            try
            {
                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
            catch (BrokerUnreachableException be)
            {
                if (retryTimes == 15)
                {
                    throw be;
                }
                retryTimes += 1;
                Thread.Sleep(5000);
                InitializeRabbitMqListener(retryTimes);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var stockMessage = JsonConvert.DeserializeObject<StockResponseMessage>(content);

                    await HandleMessage(stockMessage);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch(Exception e)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(StockResponseMessage stockResponseMessage)
        {
            var message = "";
            if (stockResponseMessage.Success)
                message = $"{stockResponseMessage.Stock.Symbol.ToUpper()} quote is ${Math.Round(stockResponseMessage.Stock.Close, 2)} per share";
            else
                message = $"Stock '{stockResponseMessage.Stock.Symbol.ToUpper()}' not found.";


            var chatViewModel = new ChatViewModel
            {
                Message = message,
                TimeStamp = DateTime.Now,
                UserId = "",
                Username = "Bot",
                RoomId = stockResponseMessage.RoomId
            };

            await _hubMethods.DispatchMessageToClients(chatViewModel);
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}