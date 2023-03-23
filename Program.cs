using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SaberTask
{
    internal class Program
    {
        private static readonly string _fileName = "ListRand.dat";
        private static readonly int _countOfNodes = 10;
        private static Random _rand = new Random();

        static void Main(string[] args)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                ListRand listRand1 = new ListRand();
                GenerateListRand(listRand1, _countOfNodes);

                using (FileStream fs = new FileStream(_fileName, FileMode.Create))
                {
                    listRand1.Serialize(fs);
                }

                ListRand listRand2 = new ListRand();

                using (FileStream fs = new FileStream(_fileName, FileMode.Open))
                {
                    fs.Close();
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

        private static void GenerateListRand(ListRand listRand, int count)
        {
            List<ListNode> listOfNodes = new List<ListNode>();
            for (int i = 0; i < count; i++)
            {
                ListNode newNode = new ListNode();
                newNode.Data = _rand.Next(0, 1000).ToString();
                if (listRand.Count == 0)
                {
                    listRand.Head = newNode;
                    listRand.Tail = newNode;
                }
                else
                {
                    newNode.Prev = listRand.Tail;
                    listRand.Tail.Next = newNode;
                    listRand.Tail = newNode;
                }
                listRand.Count++;
                listOfNodes.Add(newNode);
            }
            foreach (var node in listOfNodes)
            {
                node.Rand = listOfNodes[_rand.Next(0, listOfNodes.Count)];
            }
        }

        private static void ShowListRandInColsole(ListRand listRand)
        {
            try
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
                    sb.Append("PrevData: " + node.Prev?.Data?.ToString() + " ");
                    sb.Append("Data: " + node.Data?.ToString() + " ");
                    sb.Append("RandData: " + node.Rand?.Data?.ToString() + " ");
                    sb.Append("RandIndex: " + indexDict[node.Rand].ToString() + " ");
                    sb.Append("NextData: " + node.Next?.Data?.ToString() + " ");
                    Console.WriteLine(sb.ToString());
                    i++;
                }
                Console.WriteLine("------------------------");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Data corrupted");
            }
        }
    }
}