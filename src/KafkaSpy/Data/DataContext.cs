using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace KafkaSpy.Data
{
    public class DataContext {
        string _connectionString;
        public const string TopicTable = "TOPICS";

        private string SQLInsertTopic = $"INSERT INTO {TopicTable} (Name, Partitions, ContentType) VALUES (@Name,@Partitions,@ContentType);";
        private string SQLUpdateTopic = $"UPDATE {TopicTable} SET Name = @Name, Partitions = @Partitions, ContentType = @ContentType WHERE Id = @Id;";
        private string SQLDeleteTopic = $"DELETE FROM {TopicTable} WHERE Id = @Id;";
        private string SQLSelectTopics = $"SELECT * FROM {TopicTable};";


        public DataContext (string ConnectionString){
            _connectionString = ConnectionString;
            Init();
        }

        private void Init(){
            using (var cnn = new SqliteConnection(_connectionString)){
                cnn.Open();
                using (var create = cnn.CreateCommand()){
                    create.CommandText= $@"
                    CREATE TABLE IF NOT EXISTS {TopicTable} (
                        Id INTEGER PRIMARY KEY,
                        Name TEXT NOT NULL,
                        Partitions INTEGER,
                        ContentType INTEGER
                    );";
                    create.ExecuteNonQuery();
                };
            }
        }


        public void Add (IEnumerable<Topic> topics){
            foreach (var topic in topics)
            {
                Add(topic);
            }
        }
        public void Add(Topic topic){
            if (topic.Id > 0)
                Update(topic);
            else
                Insert(topic);
        }


        public void Insert (Topic topic){
            using (var cnn = new SqliteConnection(_connectionString)){
                cnn.Open();
                topic.Id = cnn.ExecuteScalar<int>($@"
                    begin; 
                    {SQLInsertTopic}
                    select last_insert_rowid(); 
                    commit;
                    ", topic);
                cnn.Close();
            }
        }


        public void Update(Topic topic){
            using (var cnn = new SqliteConnection(_connectionString)){
                cnn.Open();
                topic.Id = cnn.ExecuteScalar<int>(SQLUpdateTopic, topic);
                cnn.Close();
            }
        }

        public IEnumerable<Topic> GetTopics(){

            IEnumerable<Topic> topics;
            using (var cnn=new SqliteConnection(_connectionString)){
                cnn.Open();
                topics = cnn.Query<Topic> (SQLSelectTopics);
                cnn.Close();
            }
            return topics;
        }

        public IEnumerable<Topic> UpdateTopics(IEnumerable<Topic> topicsDelete, IEnumerable<Topic> topics){

            IEnumerable<Topic> output;
            using (var cnn=new SqliteConnection(_connectionString)){
                cnn.Open();
                using (var trans = cnn.BeginTransaction()){
                    cnn.Execute(SQLDeleteTopic,topicsDelete,trans);
                    cnn.Execute(SQLInsertTopic,topics.Where(x=>x.Id == 0), trans );
                    cnn.Execute(SQLUpdateTopic,topics.Where(x=>x.Id > 0), trans );
                    trans.Commit();
                };

                output = cnn.Query<Topic> (SQLSelectTopics);
                cnn.Close();
            }
            return output;

        }

        

       

    }
}