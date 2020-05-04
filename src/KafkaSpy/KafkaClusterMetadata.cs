using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Confluent.Kafka;
using KafkaSpy.Configuration;
using KafkaSpy.Data;

namespace KafkaSpy
{
    public class KafkaClusterMetadata
    {

        private IAdminClient _adminClient;
        private Metadata _metadata;
        public ClientConfig KafkaClientConfig { get; private set; }

        DataContext _dataContext;

        private IEnumerable<Topic> _topics;

        public KafkaClusterMetadata(ClientConfig kafkaClientConfig , DataContext dataContext)
        {
            KafkaClientConfig = kafkaClientConfig;
            _dataContext = dataContext;
            _topics = dataContext.GetTopics();

            var adminClientConfig = new AdminClientConfig(KafkaClientConfig);
            
            _adminClient = new AdminClientBuilder(adminClientConfig).Build();
            RefreshData(TimeSpan.FromSeconds(10));
        }

        private void RefreshData(System.TimeSpan timeout)
        {
            var topicsDb = _dataContext.GetTopics();

            _metadata = _adminClient.GetMetadata(timeout);
            var topicsKafka = _metadata.Topics.Select(t=>new Topic(){Name=t.Topic,Partitions = t.Partitions.Count});

            var topicsDelete = _topics.Except(topicsKafka);

            var topicsPersist = topicsKafka.GroupJoin(
                topicsDb,
                tk => tk.Name,
                tdb => tdb.Name,
                (tk, tdb) => new Topic {Id=(tdb.SingleOrDefault()?.Id).GetValueOrDefault(), Name=tk.Name, Partitions = tk.Partitions, ContentType=(tdb.SingleOrDefault()?.ContentType).GetValueOrDefault(TopicContentType.String)}
            );

            _topics = _dataContext.UpdateTopics(topicsDelete, topicsPersist);
        }

        public List<Topic> GetTopics()
        {
           return _topics.ToList();
        }

        public string GetBootstrapServers () {
            return KafkaClientConfig.BootstrapServers;
        }
        
    }
}