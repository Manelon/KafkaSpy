using System;
using System.Linq;
using Confluent.Kafka.Admin;
using Xunit;
using KafkaSpy;

namespace KafkaSpy.Tests
{
    public class KafkaClusterMetadataTest
    {
        private KafkaClusterMetadata _metadata;
        public KafkaClusterMetadataTest()
        {
            _metadata = new KafkaClusterMetadata("localhost:9092");
        }
    
         [Fact]
        public void Test1()
        {
            var topics = _metadata.GetTopics();
            Assert.Contains("test",topics);
            Assert.True(topics.Count == 2);

        }
    }
}
