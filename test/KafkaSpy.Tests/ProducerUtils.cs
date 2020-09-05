using System.Collections.Generic;
using Confluent.Kafka;

namespace KafkaSpy.Tests
{
    public class ProducerUtils
    {
        public static void ProduceStringMessages(string topicName, IEnumerable<TextMessaje> messajes)
        {
            var config = new ProducerConfig { BootstrapServers = TestConfig.Bootstrap };
            
            using (var producer = new ProducerBuilder<string, string>(config).Build()){

                foreach (var messaje in messajes)
                {
                   var result =  producer.ProduceAsync(topicName, new Message<string, string> { Key = messaje.Key, Value = messaje.Value }).Result;
                }
                
            }
        }
    }

    
    public class TextMessaje{
        public string Key { get; set; }
        public string Value { get; set; }
    }


}