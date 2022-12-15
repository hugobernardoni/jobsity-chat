using JobSity.Bot.Messaging.Receiver;
using JobSity.Bot.Messaging.Sender;
using JobSity.Bot.Services.Abstract;
using JobSity.Bot.Services.Implementations;
using JobSity.Model.Models.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace JobSity.Bot
{
    class Program
    {
        public static IConfigurationRoot Configuration;


        static void Main(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");            

            if (string.IsNullOrWhiteSpace(environment))
                environment = "Development";

            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);
            
            if (environment == "Development")
            {
                builder.AddJsonFile(
                        Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
                        optional: true
                    );
            }

            Console.WriteLine("builder: "+ builder);

            Configuration = builder.Build();

            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddHttpClient<IChatService, ChatService>();

                    var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
                    var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();

                    Console.WriteLine("serviceClientSettings: " + serviceClientSettingsConfig);

                    services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);
                    if (serviceClientSettings.Enabled)
                    {
                        services.AddScoped<IStockResponseSender, StockResponseSender>();
                        services.AddHostedService<StockRequestReceiver>();
                    }
                }).Build().Run();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
