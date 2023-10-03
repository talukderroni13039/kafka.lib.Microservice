using Kafka.Library;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Kafka.Consumer2
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly ILogger<BackgroundWorker> _logger;
        private KafkaConsumer _KafkaConsumer;
        public IConfiguration configRoot { get; }
        public BackgroundWorker(ILogger<BackgroundWorker> logger, KafkaConsumer kafkaConsumer, IConfiguration configuration)
        {
            _logger = logger;
            _KafkaConsumer = kafkaConsumer;
            configRoot = configuration;

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background service is starting For Consumer 2.", DateTime.Now);
            try
            {
                await consumeDataFromKafka(cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception occured!", ex.Message);
                throw ex;
            }
        }
        private async Task consumeDataFromKafka(CancellationToken cancellationToken)
        {
            var BootstrapServers = configRoot.GetValue<string>("BootstrapServers");
            var GroupId = configRoot.GetValue<string>("GroupId");
            var topics = configRoot.GetValue<string>("topics");

            _KafkaConsumer.InitializeConsumerConfig(BootstrapServers, GroupId);
            _KafkaConsumer.Subscribe(topics);

            // for multiple topic Consume onthis  


            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _KafkaConsumer.Consume(cancellationToken);
                if (consumeResult != null)
                {
                    // Process the consumed message here
                    Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                }
            }
        }
    }
}