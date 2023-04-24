using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using HashProcessorApp.RabbitMq;

namespace HashProcessorApp
{
    public class Worker : BackgroundService
    {
        private readonly RabbitMqConsumer _rabbitMqConsumer;

        public Worker(RabbitMqConsumer rabbitMqConsumer)
        {
            _rabbitMqConsumer = rabbitMqConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}
