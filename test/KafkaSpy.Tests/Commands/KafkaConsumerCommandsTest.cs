using FizzWare.NBuilder;
using KafkaSpy.Commands;
using Xunit;

namespace KafkaSpy.Tests.Commands
{
    public class KafkaConsumerCommandsTest
    {
        TemporaryTopic _tempTopic;
        long _topicCountSize = 100L;
        public KafkaConsumerCommandsTest(){
            _tempTopic =  new TemporaryTopic("ConsumerCommandTest", 2);
            
             
            var messajes = Builder<TextMessaje>.CreateListOfSize((int) _topicCountSize)
                .All()
                    .With(c=>c.Key =Faker.Identification.UkNationalInsuranceNumber())
                    .With(c=>c.Value = Faker.Name.FullName())
                .Build();

            ProducerUtils.ProduceStringMessages(_tempTopic.Name, messajes);
        }

        [Fact]
        public void Can_count_message_in_topic(){
            var count = KafkaConsumer.CountTopicMessajes(TestConfig.Bootstrap, "TestCount", _tempTopic.Name);

            Assert.Equal( _topicCountSize, count);
        }
    }
}