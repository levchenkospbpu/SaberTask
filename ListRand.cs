using System.IO;

namespace SaberTask
{
    internal class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            ListRandSerializer serializer = new ListRandSerializer(this);
            serializer.SerializeHandler(s);
        }

        public void Deserialize(FileStream s)
        {
            ListRandDeserializer deserializer = new ListRandDeserializer(this);
            deserializer.DeserializeHandler(s);
        }
    }
}
