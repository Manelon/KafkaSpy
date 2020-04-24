using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaSpy.Tests
{
    public class TemporaryTopic
    {
        private string bootstrapServers;
        
        public string Name { get; set; }

        public TemporaryTopic(string bootstrapServers, int numPartitions)
        {
            this.bootstrapServers = bootstrapServers;
            this.Name = "kafkaspy_test_" + Guid.NewGuid().ToString();

            var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
            adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                new TopicSpecification { Name = Name, NumPartitions = numPartitions, ReplicationFactor = 1 } }).Wait();
            adminClient.Dispose();
        }

        public void Dispose()
        {
            var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = this.bootstrapServers }).Build();
            adminClient.DeleteTopicsAsync(new List<string> { Name }).Wait();
            adminClient.Dispose();
        }
    }
}