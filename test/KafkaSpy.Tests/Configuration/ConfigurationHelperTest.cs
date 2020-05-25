using System.IO;
using KafkaSpy.Configuration;
using Xunit;

namespace KafkaSpy.Tests.Configuration
{
    public class ConfigurationHelperTest
    {
        [Fact]
        public void Get_Array_of_Configuration() {
            var args = new string[0];
            var config = ConfigurationHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory(),"appsettings-test.json", args);

            var kafkaSection = config.GetSection("Kafka");

            
        }
    }
}