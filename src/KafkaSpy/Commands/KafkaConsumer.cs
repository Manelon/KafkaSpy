using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace KafkaSpy.Commands
{
    public class CountTopicMessajesRestult{
        public long Count { get; set; }
        public TimeSpan Elapsed { get; set; }

        public override String ToString(){
            return $"Messajes {Count} in {Elapsed.TotalSeconds} seconds";
        }
    }

    public class KafkaConsumer
    {
        public static CountTopicMessajesRestult CountTopicMessajes(string bootstrapServers, string consumerGroup, string topicName, int stepProgress, IProgress<CountTopicMessajesRestult> progress)
        {
            var config = new ConsumerConfig()
            {
                GroupId = consumerGroup,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            
            long count = 0;
            Stopwatch sw = Stopwatch.StartNew(); 
            using (var c = new ConsumerBuilder<Ignore, Ignore>(config).Build())
            {
                c.Subscribe(topicName);
                // var partitionStatus = Enumerable.Repeat(false, c.Assignment.Count).ToArray();
                var timeout = TimeSpan.FromSeconds(20);
                ConsumeResult<Ignore, Ignore> result;
                try
                {
                    while (true)
                    {
                        result = c.Consume(timeout);
                        if (result == null)
                        {
                            break;
                        }
                        else
                        {
                            if (count == 0)
                                timeout = TimeSpan.FromSeconds(1);
                            count++;

                             if (progress != null && (count % stepProgress ==0) )
                            {
                                progress.Report(new CountTopicMessajesRestult(){Count=count,Elapsed=sw.Elapsed});
                            }   
 
                        }
                    }
                }
                finally
                {
                    c.Close();
                }

                var countResult = new CountTopicMessajesRestult(){Count=count,Elapsed=sw.Elapsed};
                sw.Stop();

                return countResult;
            }
        }
        
    }
}