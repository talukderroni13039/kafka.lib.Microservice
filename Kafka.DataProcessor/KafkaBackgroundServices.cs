using Confluent.Kafka;
using Kafka.Library;
using Microsoft.Extensions.Hosting;

namespace Kafka.DataProcessor
{
    public class KafkaBackgroundServices : IHostedService
    {
        private Timer _timer;
        private readonly ILogger<KafkaBackgroundServices> _logger;
        
        private KafkaConsumer _KafkaConsumer;
        public IConfiguration configRoot { get; }
        public KafkaBackgroundServices(ILogger<KafkaBackgroundServices> logger, KafkaConsumer kafkaConsumer, IConfiguration configuration)
        {
            _logger = logger;
            _KafkaConsumer= kafkaConsumer;
            configRoot = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
                _logger.LogInformation("Background service is starting For Consumer 1.", DateTime.Now);
                try
                {
                    await consumeDataFromKafkaAsync(cancellationToken);

                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Exception occured!", ex.Message);
                    throw ex;
                }
           
        }

        private  async Task consumeDataFromKafkaAsync(CancellationToken cancellationToken)
        {
            var BootstrapServers = configRoot.GetValue<string>("BootstrapServers");
            var GroupId = configRoot.GetValue<string>("GroupId");
            var topics = configRoot.GetValue<string>("topics");

            _KafkaConsumer.InitializeConsumerConfig(BootstrapServers, GroupId);
            _KafkaConsumer.Subscribe(topics);

            // for multiple topic Consume onthis  


          while (!cancellationToken.IsCancellationRequested)
          {
            var consumeResult =  _KafkaConsumer.Consume(cancellationToken);
                /* this is crucial and acts as thread.
                like it will only consume when new messsage produce from topic and it await 
                that means thread await here untill new message under the while loop.
                cancellationToken-- wait for new message.
                Timespan- wait for specitime 
                */

                if (consumeResult != null)
                 {
                // Process the consumed message here
                   Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                }
           }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}