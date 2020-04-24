using System;
using System.Linq;
using Confluent.Kafka.Admin;
using Xunit;
using KafkaSpy;

namespace KafkaSpy.Tests
{
    public class KafkaClusterMetadataTest : IDisposable
    {
        private KafkaClusterMetadata _metadata;
        TemporaryTopic _tempTopic;

        public KafkaClusterMetadataTest()
        {
            _tempTopic =  new TemporaryTopic(TestConfig.Bootstrap, 1);
            

            _metadata = new KafkaClusterMetadata(TestConfig.Bootstrap);
            
            
        }

        public void Dispose()
        {
           _tempTopic.Dispose();
        }

        [Fact]
        public void Test1()
        {
            var topics = _metadata.GetTopics();
            Assert.Contains(_tempTopic.Name,topics);
            

        }

    }
}
