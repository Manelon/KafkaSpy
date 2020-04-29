using System;
using System.Linq;
using Confluent.Kafka.Admin;
using Xunit;
using KafkaSpy;
using Microsoft.Data.Sqlite;
using KafkaSpy.Data;

namespace KafkaSpy.Tests
{
    public class KafkaClusterMetadataTest : IDisposable
    {
        private KafkaClusterMetadata _metadata;
        TemporaryTopic _tempTopic;
        SqliteConnection _cnn;
        DataContext _dataContext;

        public KafkaClusterMetadataTest()
        {
            _tempTopic =  new TemporaryTopic(1);

            _cnn = new SqliteConnection(TestConfig.ConnectionString);
            _cnn.Open();//Keep the connection open in order to share the inmemory database in all the tests
            _dataContext = new DataContext(TestConfig.ConnectionString);
            

            _metadata = new KafkaClusterMetadata(TestConfig.Bootstrap, _dataContext);
            
            
        }

        public void Dispose()
        {
           _tempTopic.Dispose();
           _cnn.Close();
        }

        [Fact]
        public void CheckIfIsAbleToGetTopicInformation()
        {
            var topics = _metadata.GetTopics();
            Assert.True(topics.Exists(x=>x.Name == _tempTopic.Name));
            

        }

    }
}
