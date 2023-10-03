
using Kafka.DataProcessor;
using Kafka.Library;
using Microsoft.Extensions.Hosting;

namespace Kafka.Consumer1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    configureServices(services);

                    services.AddHostedService<KafkaBackgroundServices>();
                });


        private static void configureServices(IServiceCollection services)
        {
            services.AddTransient<KafkaConsumer>();

        }
    }
}