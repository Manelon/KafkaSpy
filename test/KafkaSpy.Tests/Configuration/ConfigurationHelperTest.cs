using System.IO;
using KafkaSpy.Configuration;
using Xunit;

namespace KafkaSpy.Tests.Configuration
{
    public class ConfigurationHelperTest
    {
        [Fact]
        public void Is_able_to_read_configuration_from_file()
        {
            var args = new string[0];
            var config = ConfigurationHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory(), "appsettings-test.json", args);

            var kafkaConfiguration = config.GetKafkaConfiguration();
            Assert.Equal("localhost:9092", kafkaConfiguration.BootstrapServers);
        }
    }
}