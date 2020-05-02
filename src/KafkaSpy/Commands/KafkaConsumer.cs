using System;
using Confluent.Kafka;

namespace KafkaSpy.Commands
{
    public class KafkaConsumer
    {
        public static long CountTopicMessajes(string bootstrapServers, string consumerGroup, string topicName)
        {
            var config = new ConsumerConfig()
            {
                GroupId = consumerGroup,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            long count = 0;
            using (var c = new ConsumerBuilder<Ignore, Ignore>(config).Build())
            {
                c.Subscribe(topicName);
                var timeout = TimeSpan.FromSeconds(20);
                ConsumeResult<Ignore,Ignore> result;
                try
                {
                    while (true)
                    {
                        result = c.Consume(timeout);
                        if (result == null)
                            break;
                        count++;
                        timeout = TimeSpan.FromSeconds(1); //The first connection usually takes more than the rest
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    c.Close();
                }

                return count;
            }
        }
    }
}