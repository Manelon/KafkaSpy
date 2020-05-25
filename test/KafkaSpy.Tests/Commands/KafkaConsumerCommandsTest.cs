using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using KafkaSpy.Commands;
using Xunit;

namespace KafkaSpy.Tests.Commands
{
    public class KafkaConsumerCommandsTest
    {
        TemporaryTopic _tempTopic;
        long _topicCountSize = 1000;
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
            var stepProgress = 50;
            var progressResults = new List<CountTopicMessajesRestult>();
            var progress = new Progress<CountTopicMessajesRestult>(r=>{Console.WriteLine (r);
                                                                progressResults.Add(r);});


            var count = KafkaConsumer.CountTopicMessajes(TestConfig.BuildKafkaClientConfig(), "TestCount", _tempTopic.Name, stepProgress, progress);

            //Check toal
            Assert.Equal( _topicCountSize, count.Count);
            Console.WriteLine(count);
            if (count.Count > 1)
                Assert.Equal( (_topicCountSize/stepProgress)+1, progressResults.Count);
        }
    }
}