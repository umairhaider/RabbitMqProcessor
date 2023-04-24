using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HashProcessorApp.Repositories;
using HashProcessorApp.Models;
using HashProcessorApp.Services;

namespace HashProcessorApp.RabbitMq
{
    public class RabbitMqConsumer : IDisposable
    {
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AsyncEventingBasicConsumer _consumer;
        private readonly string _queueName;

        public RabbitMqConsumer(IConnection connection, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _channel = connection.CreateModel();
            _queueName = "hashes";

            _channel.QueueDeclare(queue: _queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += OnReceivedAsync;

            _channel.BasicConsume(queue: _queueName,
                                  autoAck: false,
                                  consumer: _consumer);
        }

        private async Task OnReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var hashProcessor = scope.ServiceProvider.GetRequiredService<HashProcessor>();

                var sha1 = Encoding.UTF8.GetString(e.Body.ToArray());
                await hashProcessor.ProcessMessage(sha1);

                _channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
