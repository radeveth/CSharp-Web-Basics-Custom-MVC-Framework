using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            // Demo 01
            //Task evenrNumbersTasks = Task.Run(() => 
            //{
            //    for (int i = 0; i < 100; i+=2)
            //    {
            //        Console.WriteLine(i);
            //        Thread.Sleep(100);
            //    }
            //});

            //Task oddNumbersTasks = Task.Run(() => 
            //{
            //    for (int i = 1; i < 99; i+=2)
            //    {
            //        Console.WriteLine(i);
            //        Thread.Sleep(100);
            //    }
            //});

            //Task.WaitAll(evenrNumbersTasks, oddNumbersTasks);



            // Demo 02
            //Task<int> sum = Task.Run(() => 
            //{
            //    int sum = 0;

            //    for (int i = 0; i < 1000; i++)
            //    {
            //        sum += i;
            //    }

            //    return sum;
            //});

            //Console.WriteLine("Suming");
            //for (int i = 0; i < 100; i++)
            //{
            //    Console.Write(".");
            //    Thread.Sleep(10);
            //}

            //Console.WriteLine($"{Environment.NewLine}Result: {sum.Result}");


            // Demo 03
            //int sum = 0;
            //Task task = Task.Run(() => 
            //{
            //    for (int i = 0; i < 1000; i++)
            //    {
            //        sum += i;

            //        if (i % 10 == 0)
            //        {
            //            Console.Write(".");
            //            Thread.Sleep(100);
            //        }
            //    }
            //});

            //while (true)
            //{
            //    string line = Console.ReadLine();

            //    if (line == "show")
            //    {
            //        Console.WriteLine($"Current result: {sum}");
            //    }
            //    else if (line == "exit")
            //    {
            //        Console.WriteLine($"Final sum: {sum}");
            //        return;
            //    }
            //}


            // Demo 04
            //Task<int> task = Task.Run(() =>
            //{
            //    int sum = 0;
            //    for (int i = 0; i < 1000; i++)
            //    {
            //        sum += i;
            //    }

            //    return sum;
            //}).ContinueWith(task => 
            //{
            //    return task.Result / 10;
            //}).ContinueWith(task => 
            //{ 
            //    return task.Result / 10;
            //}).ContinueWith(task => 
            //{
            //    return task.Result / 10;
            //});

            //Console.WriteLine(task.Result);

            // Demo 05
            //Task<int> task = DoSomeWork();
            //var result = task.Result;

            var list = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                int currentValue = i;

                var task = Task.Run(() => 
                {
                    Console.WriteLine(currentValue);
                });

                list.Add(task); 
            }

            await Task.WhenAll(list);

            Console.WriteLine("Done!");
        }

        private static Task<int> DoSomeWork()
        {
            return Task.Run(() => 
            {
                // Long operation...
                return 100;
            });
        }
    }
}
