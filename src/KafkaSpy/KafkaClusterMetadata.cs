using System;
using System.Collections.Immutable;
using System.Linq;
using Confluent.Kafka;


namespace KafkaSpy
{
    public class KafkaClusterMetadata
    {

        private IAdminClient _adminClient;
        private Metadata _metadata;
        private string _bootstrapServers;

        public KafkaClusterMetadata(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;
            var adminClientConfig = new AdminClientConfig()
            {
                BootstrapServers =  _bootstrapServers
            };
            
            _adminClient = new AdminClientBuilder(adminClientConfig).Build();
            _metadata = _adminClient.GetMetadata(TimeSpan.FromSeconds(20));
        }

        public ImmutableList<string> GetTopics()
        {
           return _metadata.Topics.Select(t => t.Topic).ToImmutableList();
        }

        public string GetBootstrapServers () {
            return _bootstrapServers;
        }
        
        
        
    }
}