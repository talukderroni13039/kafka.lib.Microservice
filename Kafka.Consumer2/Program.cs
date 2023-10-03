using Kafka.Library;

namespace Kafka.Consumer2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    configureServices(services);
                    services.AddHostedService<BackgroundWorker>();
                })
                .Build();

            host.Run();
        }
        private static void configureServices(IServiceCollection services)
        {
            services.AddTransient<KafkaConsumer>();

        }
    }
}