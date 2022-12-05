using JobSity.Bot.Messaging.Sender;
using JobSity.Bot.Services.Abstract;
using JobSity.Model.Models.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JobSity.Bot.Messaging.Receiver
{
    public class StockRequestReceiver : BackgroundService
    {
        private readonly IStockResponseSender _stockResponseSender;
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly int _port;
        private IChatService _chatService;
        private readonly string _username;
        private readonly string _password;

        public StockRequestReceiver(IOptions<RabbitMqConfiguration> rabbitMqOptions,
                                    IStockResponseSender stockResponseSender,
                                    IChatService chatService)
        {
            _stockResponseSender = stockResponseSender;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _port = rabbitMqOptions.Value.Port;
            _chatService = chatService;
            _queueName = "StockRequest";

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


            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
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
                    var stockRequestMessage = JsonConvert.DeserializeObject<StockRequestMessage>(content);

                    Console.WriteLine($"Checking stock for '{stockRequestMessage.Code}'");

                    var stock = await HandleMessage(stockRequestMessage);
                    _channel.BasicAck(ea.DeliveryTag, false);
                    _stockResponseSender.SendStockResponse(stock);
                }
                catch (Exception e)
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

        private async Task<StockResponseMessage> HandleMessage(StockRequestMessage stockRequestMessage)
        {
            var stockResponseMessage = new StockResponseMessage();

            try
            {
                stockResponseMessage = await _chatService.GetStockDetails(stockRequestMessage.Code);

                Console.WriteLine($"Stock '{stockRequestMessage.Code}' - Close: {stockResponseMessage.Stock.Close}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for '{stockRequestMessage.Code}'");
                stockResponseMessage.Success = false;
            }

            stockResponseMessage.RoomId = stockRequestMessage.RoomId;
            stockResponseMessage.Stock.Symbol = stockRequestMessage.Code;
            return stockResponseMessage;
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
