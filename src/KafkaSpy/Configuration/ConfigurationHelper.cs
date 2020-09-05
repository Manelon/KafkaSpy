using System;
using System.IO;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace KafkaSpy.Configuration
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string configurationPath, string[] args)
        {
            return new ConfigurationBuilder()
                .SetBasePath(configurationPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args) //usging arguments line shoulf be --key=value for instance Kafka:BootstrapServers:localhost:8082
                .Build();
        }

        public static IConfigurationRoot GetIConfigurationRoot(string configurationPath, string jsonFileName, string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile(jsonFileName, optional: true, reloadOnChange: true)
                .AddCommandLine(args) //usging arguments line shoulf be --key=value for instance Kafka:BootstrapServers:localhost:8082
                .Build();
        }

        public static ClientConfig GetKafkaConfiguration(this IConfiguration configuration)
        {
            var kafkaConf = new ClientConfig();
            configuration.GetSection("Kafka")
                .Bind(kafkaConf);
            return kafkaConf;
        }

        
    }
}