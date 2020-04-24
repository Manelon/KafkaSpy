using System;
using System.IO;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace KafkaSpy.Configuration
{
    public class ConfigurationHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string configurationPath, string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args) //usging arguments line shoulf be --key=value for instance Kafka:BootstrapServers:localhost:8082
                .Build();
        }

        public static Kafka GetKafkaConfiguration(string configurationPath, string[] args)
        {
            var configuration = new Kafka();
            var iConfig = GetIConfigurationRoot(configurationPath, args);
            iConfig.GetSection("Kafka")
                .Bind(configuration);
            return configuration;


        }
        
    }
}