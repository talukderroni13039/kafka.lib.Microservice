using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka.Library
{
    public class KafkaConsumer : IDisposable
    {
        private  IConsumer<string, string> _consumer;

        // initialize consumer
        public void InitializeConsumerConfig(string bootstrapServers, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers, // docker running server
                GroupId = groupId, // set different group id for mutiple consumer simultaneously.
                AutoOffsetReset = AutoOffsetReset.Earliest,// read the topic message from begin.
                EnableAutoCommit = true// after message read it will not read again if it is true
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }
        public void Subscribe(string topic)
        {
            _consumer.Subscribe(topic);
        }

        public  ConsumeResult<string, string>  Consume(CancellationToken cancellationToken )
        {
            return _consumer.Consume(cancellationToken);
        }
        public void Commit(ConsumeResult<string, string> result)
        {
            _consumer.Commit(result); // Commit the offset for the consumed message
        }
        public void Close()
        {
            _consumer.Close();
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
