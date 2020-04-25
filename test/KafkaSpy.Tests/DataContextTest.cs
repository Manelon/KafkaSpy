using System;
using Dapper;
using KafkaSpy.Data;
using Microsoft.Data.Sqlite;
using Xunit;
using System.Linq;

namespace KafkaSpy.Tests
{
    public class DataContextTest : IDisposable
    {
        SqliteConnection _cnn;
        DataContext _dataContext;

        

        public DataContextTest()
        {
            _cnn = new SqliteConnection(TestConfig.ConnectionString);
            _cnn.Open();//Keep the connection open in order to share the inmemory database in all the tests
            _dataContext = new DataContext(TestConfig.ConnectionString);
        }

        public void Dispose()
        {
            _cnn.Close();
            _cnn.Dispose();
        }
        [Fact]
        public void Database_should_have_tables_defined_in_DataContext(){
            var tables = _cnn.Query<string>($"SELECT name FROM sqlite_master WHERE type='table'").AsList();
            Assert.Contains<String>(DataContext.TopicTable, tables);   
        }

       [Fact]
       public void Topics_should_be_stored (){
           var topic = new Topic(){
               Name = "TopicTest",
               Partitions = 1,
               ContentType = TopicContentType.String
           };

           _dataContext.Add(topic);

           Assert.NotEqual(0, topic.Id);

           var topicDb= _cnn.QuerySingle<Topic>($"Select * from {DataContext.TopicTable} where Id = @Id", new{Id = topic.Id});

            Assert.True(topic.Name == topicDb.Name && topic.Id == topicDb.Id);
        }

        [Fact]
        public void Update_topics_works (){
            Topic[] topicsInit = {
                new Topic(){ Name = "Delete1", Partitions = 1, ContentType = TopicContentType.String},
                new Topic(){ Name = "Delete2", Partitions = 1, ContentType = TopicContentType.String},
                new Topic(){ Name = "Persisted1", Partitions = 1, ContentType = TopicContentType.String},
                new Topic(){ Name = "Persisted2", Partitions = 1, ContentType = TopicContentType.String}
           };

           _dataContext.Add(topicsInit);

           var topicsBD = _dataContext.GetTopics();

            Assert.Equal(topicsBD.Join(
                topicsInit,
                db => db.Name,
                i => i.Name,
                (db,i) => db
            ).Count(), topicsInit.Count());

           var topicsDelete = topicsBD.Where(x=>x.Name.Contains("Delete"));
           var topicsPersist = topicsBD.Where(x=>x.Name.Contains("Persisted"))
                        .Append(new Topic(){ Name = "Insert", Partitions = 1, ContentType = TopicContentType.String});

            var topicsFinal = _dataContext.UpdateTopics(topicsDelete, topicsPersist);

            Assert.Equal(topicsFinal.Join(
                topicsPersist,
                f=>f.Name,
                p=>p.Name,
                (f,p)=>(f)
            ).Count(), topicsFinal.Count());
            
            
        }       
    }
}