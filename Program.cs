using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SaberTask
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRand
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
                temp.Rand = list[rand.Next(0,Count)];
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
            catch (IOException)
            {
                Console.WriteLine("An I/O error occurred");
            }
        }
    }

    class Program
    {
        public static readonly string fileName = "ListRand.dat";
        public static readonly int countOfNodes = 100000;
        public static Random rand = new Random();

        static void ShowListRandInColsole(ListRand listRand)
        {
            Dictionary<ListNode, int> indexDict = new Dictionary<ListNode, int>();

            int i = 0;
            ListNode temp = listRand.Head;
            while (temp != null)
            {
                indexDict.Add(temp, i);
                temp = temp.Next;
                i++;
            }

            i = 0;
            Console.WriteLine("------------------------");
            StringBuilder sb = new StringBuilder();
            foreach (ListNode node in indexDict.Keys)
            {
                sb.Clear();
                sb.Append(i.ToString() + ") ");
                sb.Append("PrevData: " + node.Prev?.Data.ToString() + " ");
                sb.Append("Data: " + node.Data.ToString() + " ");
                sb.Append("RandData: " + node.Rand.Data.ToString() + " ");
                sb.Append("RandIndex: " + indexDict[node.Rand].ToString() + " ");
                sb.Append("NextData: " + node.Next?.Data.ToString() + " ");
                Console.WriteLine(sb.ToString());
                i++;
            }
            Console.WriteLine("------------------------");
        }

        static void Main(string[] args)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                ListRand listRand1 = new ListRand();
                for (int i = 0; i < countOfNodes; i++)
                {
                    listRand1.CreateNode(rand.Next(0, 1000).ToString());
                }
                listRand1.SetRandRefs();

                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    listRand1.Serialize(fs);
                }

                ListRand listRand2 = new ListRand();

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    listRand2.Deserialize(fs);
                }

                ShowListRandInColsole(listRand1);
                ShowListRandInColsole(listRand2);

                stopwatch.Stop();
                Console.WriteLine(stopwatch.Elapsed);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("File name is zero-length, contains only spaces, or contains one or more invalid characters");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("The specified path, filename, or both, exceeds the system's maximum length");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid path specified");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Unauthorized access error occurred while opening the file");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The filename is in an invalid format");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}