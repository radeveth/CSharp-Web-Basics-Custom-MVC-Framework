using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Threads
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Demo 01
            Console.WriteLine("Downloading File! Enter exit to cancel!");

            Thread thread = new Thread(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(100);
                }
            });

            thread.Start();
            Console.WriteLine("Working");
            thread.Join();
            Console.WriteLine("Finish");

            string command = Console.ReadLine();

            if (command.ToLower() == "exit")
            {
                return;
            }


            // Demo 02
            List<int> numbers = Enumerable.Range(0, 1000).ToList();

            for (int i = 0; i < 4; i++)
            {
                new Thread(() => 
                {
                    lock (numbers)
                    {
                        while (numbers.Count > 0)
                        {
                            numbers.RemoveAt(numbers.Count - 1);
                        }
                    }
                }).Start();
            }
        }
    }
}
