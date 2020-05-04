using Confluent.Kafka;
using Microsoft.Data.Sqlite;

namespace KafkaSpy.Tests
{
    public class TestConfig
    {
        public const string Bootstrap = "localhost:29092";
        public static string ConnectionString = "Data Source=InMemoryKafkaSpy;Mode=Memory;Cache=Shared";

        public static ClientConfig BuildKafkaClientConfig(){
            return new ClientConfig(){
                BootstrapServers = Bootstrap
            };
        }
    }
}