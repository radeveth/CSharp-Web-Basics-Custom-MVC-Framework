using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelDemo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Parallel.For(1, 100, number =>
            {
                Console.WriteLine(number);
            });


            Console.WriteLine();
            Console.WriteLine(new string('-', 100));
            Console.WriteLine(new string('-', 100));
            Console.WriteLine(new string('-', 100));
            Console.WriteLine();
            
            
            var lsit = Enumerable.Range(0, 1000).ToList();
            Parallel.ForEach(lsit, el => 
            {
                Console.WriteLine(el);
            });
        }
    }
}
