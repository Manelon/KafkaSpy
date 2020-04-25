using System;

namespace KafkaSpy.Data
{
    public class Topic:IEquatable<Topic>
    {
        public int Id {get; set;}
        public string Name { get; set; }
        public int Partitions { get; set; }
        public TopicContentType ContentType { get; set; }

        bool IEquatable<Topic>.Equals(Topic other)
        {
            if (other is null)
                return false;
            else
                //A topic name in a kafka cluster un unique
                return this.Name == other.Name;
        }
        
        public override bool Equals(object obj) => Equals(obj as Topic);
        public override int GetHashCode() => (Name).GetHashCode();
    }

    public enum TopicContentType {
        String=1, Binary=2, Avro=3
    }
    
}