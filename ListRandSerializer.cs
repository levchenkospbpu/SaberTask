using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberTask
{
    internal class ListRandSerializer : ISerializer
    {
        private readonly char _separator = '\0';
        private ListRand _list;

        public ListRandSerializer(ListRand list)
        {
            _list = list;
        }

        private byte[] CreateData(Dictionary<ListNode, int> indexDict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListNode node in indexDict.Keys)
            {
                sb.Append(node.Data);
                sb.Append(_separator);
                sb.Append(node.Prev == null ? "null" : indexDict[node.Prev].ToString());
                sb.Append(_separator);
                sb.Append(node.Next == null ? "null" : indexDict[node.Next].ToString());
                sb.Append(_separator);
                sb.Append(node.Rand == null ? "null" : indexDict[node.Rand].ToString());
                sb.Append(_separator);

            }
            return UnicodeEncoding.UTF8.GetBytes(sb.ToString());
        }

        private Dictionary<ListNode, int> ListRandToDict(ListRand list)
        {
            Dictionary<ListNode, int>  indexDict = new Dictionary<ListNode, int>();
            int i = 0;
            ListNode temp = _list.Head;
            while (temp != null)
            {
                indexDict.Add(temp, i);
                temp = temp.Next;
                i++;
            }
            return indexDict;
        }

        public void SerializeHandler(FileStream fileStream)
        {
            try
            {
                Dictionary<ListNode, int> indexDict = ListRandToDict(_list);
                fileStream.Write(CreateData(indexDict));
            }
            catch (IOException)
            {
                Console.WriteLine("An I/O error occurred");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Data corrupted");
            }
        }
    }
}
