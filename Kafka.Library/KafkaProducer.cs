using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Library
{
    public class KafkaProducer:IDisposable
    {
        private  IProducer<string, string> _producer;
        public  void InitializeProduce(string bootstrapServers, string clientId)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = clientId,
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }
        public void Produce(string topic, Message<string, string> message)
        {
            _producer.Produce(topic,message);
            _producer.Flush(TimeSpan.FromMinutes(10));
        }
        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
