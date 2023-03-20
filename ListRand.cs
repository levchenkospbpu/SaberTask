using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberTask
{
    internal class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public ListNode CreateNode(string data = null)
        {
            ListNode newNode = new ListNode();
            newNode.Data = data;
            if (Count == 0)
            {
                Head = newNode;
                Tail = newNode;
            }
            else
            {
                newNode.Prev = Tail;
                Tail.Next = newNode;
                Tail = newNode;
            }
            Count++;
            return newNode;
        }

        public void AddNode(ListNode node)
        {
            if (Count == 0)
            {
                Head = node;
                Tail = node;
            }
            else
            {
                node.Prev = Tail;
                Tail.Next = node;
                Tail = node;
            }
            Count++;
        }

        public List<ListNode> ToList()
        {
            List<ListNode> list = new List<ListNode>();
            ListNode temp = Head;
            while (temp != null)
            {
                list.Add(temp);
                temp = temp.Next;
            }
            return list;
        }

        //Sets refs to Rand for every node
        public void SetRandRefs()
        {
            Random rand = new Random();
            List<ListNode> list = this.ToList();
            ListNode temp = Head;
            while (temp != null)
            {
                temp.Rand = list[rand.Next(0, Count)];
                temp = temp.Next;
            }
        }

        public void Serialize(FileStream s)
        {
            try
            {
                Dictionary<ListNode, int> indexDict = new Dictionary<ListNode, int>();

                int i = 0;
                ListNode temp = Head;
                while (temp != null)
                {
                    indexDict.Add(temp, i);
                    temp = temp.Next;
                    i++;
                }

                StringBuilder sb = new StringBuilder();
                using (StreamWriter writer = new StreamWriter(s))
                {
                    foreach (ListNode node in indexDict.Keys)
                    {
                        sb.Clear();
                        sb.Append(indexDict[node.Rand].ToString());
                        sb.Append("|");
                        sb.Append(node.Data);
                        sb.Append(node.Next == null ? string.Empty : "\n");
                        writer.Write(sb.ToString());
                    }
                }
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

        public void Deserialize(FileStream s)
        {
            try
            {
                List<ListNode> list = new List<ListNode>();

                ListNode temp;
                string line = string.Empty;
                using (StreamReader reader = new StreamReader(s))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        temp = CreateNode();
                        temp.Data = line;
                        list.Add(temp);
                    }
                }

                int separatorIndex = 0;
                foreach (ListNode node in list)
                {
                    separatorIndex = node.Data.IndexOf("|");
                    node.Rand = list[Convert.ToInt32(node.Data.Substring(0, separatorIndex))];
                    node.Data = node.Data.Remove(0, separatorIndex + 1);
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Not enough memory to allocate a buffer for the returned string");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("File corrupted");
            }
            catch (FormatException)
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
