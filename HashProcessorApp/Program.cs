using System;
using System.Threading.Tasks;
using HashProcessorApp.DAL;
using HashProcessorApp.RabbitMq;
using HashProcessorApp.Repositories;
using HashProcessorApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace HashProcessorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<HashDbContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    services.AddScoped<IHashRepository, HashRepository>();
                    services.AddScoped<HashProcessor>();

                    services.AddSingleton<ConnectionFactory>(sp => new ConnectionFactory
                    {
                        Uri = new Uri(hostContext.Configuration.GetConnectionString("RabbitMqConnection")),
                        DispatchConsumersAsync = true
                    });

                    services.AddSingleton<IConnection>(sp => sp.GetRequiredService<ConnectionFactory>().CreateConnection());
                    services.AddSingleton<RabbitMqConsumer>();

                    services.AddHostedService<Worker>();
                });

    }
}
