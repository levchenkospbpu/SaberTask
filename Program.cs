using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SaberTask
{
    internal class Program
    {
        public static readonly string fileName = "ListRand.dat";
        public static readonly int countOfNodes = 10;
        public static Random rand = new Random();

        static void ShowListRandInColsole(ListRand listRand)
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