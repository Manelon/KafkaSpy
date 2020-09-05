using System.IO;
using Confluent.Kafka;
using KafkaSpy.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace KafkaSpy.Tests
{
    static public class TestConfig
    {
        public static readonly string Bootstrap; //= "localhost:29092";
        public static readonly string ConnectionString; //= "Data Source=InMemoryKafkaSpy;Mode=Memory;Cache=Shared";

        public static readonly ClientConfig kafkaClientConfig;

        static TestConfig()
        {
            var args = new string[0];
            var config = ConfigurationHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory(), "appsettings-test.json", args);
            kafkaClientConfig = config.GetKafkaConfiguration();
            Bootstrap = kafkaClientConfig.BootstrapServers;
            ConnectionString = config.GetConnectionString("Sqlite");
        }
    }
}