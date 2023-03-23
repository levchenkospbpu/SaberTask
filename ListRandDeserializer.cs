using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberTask
{
    internal class ListRandDeserializer : IDeserializer
    {
        private readonly char _separator = '\0';
        private readonly int _separatorCount = 4;
        private ListRand _list;

        public ListRandDeserializer(ListRand list)
        {
            _list = list;
        }

        private string GetString(FileStream fileStream)
        {
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        private List<ListNode> CreateListOfNodes(int count)
        {
            List<ListNode> listOfNodes = new List<ListNode>();
            for (int i = 0; i < count; i++)
            {
                listOfNodes.Add(new ListNode());
            }
            return listOfNodes;
        }

        private void InitListRand(string[] words, List<ListNode> listOfNodes)
        {
            int count = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (i % _separatorCount == 0)
                {
                    count++;
                }

                if (words[i] == string.Empty)
                {
                    continue;
                }

                ListNode node = listOfNodes[count - 1];

                switch (i % _separatorCount)
                {
                    case 0:
                        node.Data = words[i];
                        break;
                    case 1:
                        node.Prev = words[i] == "null" ? null : listOfNodes[Convert.ToInt32(words[i])];
                        break;
                    case 2:
                        node.Next = words[i] == "null" ? null : listOfNodes[Convert.ToInt32(words[i])];
                        break;
                    case 3:
                        node.Rand = words[i] == "null" ? null : listOfNodes[Convert.ToInt32(words[i])];
                        break;

                }
            }
            _list.Count = listOfNodes.Count;
            if (_list.Count == 0)
            {
                return;
            }
            else
            {
                _list.Head = listOfNodes[0];
                _list.Tail = listOfNodes[listOfNodes.Count - 1];
            }
        }

        public void DeserializeHandler(FileStream fileStream)
        {
            try
            {
                string text = GetString(fileStream);
                string[] words = text.Split(_separator);
                var nodeCount = words.Length / _separatorCount;
                List<ListNode> listOfNodes = CreateListOfNodes(nodeCount);
                InitListRand(words, listOfNodes);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("File corrupted");
            }
            catch (IOException)
            {
                Console.WriteLine("I/O error occurred");
            }
        }
    }
}
