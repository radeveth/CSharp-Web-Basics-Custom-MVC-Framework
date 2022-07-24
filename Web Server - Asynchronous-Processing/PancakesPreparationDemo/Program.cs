using System;
using System.Threading;
using System.Threading.Tasks;

namespace PancakesPreparationDemo
{
    public class Program
    {
        static async void Main(string[] args)
        {
            await Task.WhenAll(GetLargeBowl(), GetFlour(), GetSugar(), GetBakingPowder());
            await Task.WhenAll(MixTheDryProducts());
            await Task.WhenAll(GetMilk(), GetEggs(), GetOil());
            await Task.WhenAll(MixTheLiquidProducts(), MixAllProducts());
            await Task.WhenAll(Baking());
        }

        public static async Task GetLargeBowl()
        {
            await Task.Delay(1000);
            Console.WriteLine("Step 1: Get Large Bowl - Done!");
        }
        public static async Task GetFlour()
        {
            await Task.Delay(1000);
            Console.WriteLine("Step 2: Get Flour - Done!");
        }
        public static async Task GetSugar()
        {
            await Task.Delay(1000);
            Console.WriteLine("Step 3: Get Sugar - Done!");
        }
        public static async Task GetBakingPowder()
        {
            await Task.Delay(1000);
            Console.WriteLine("Step 4: Get Baking Powder - Done!");
        }
        public static async Task MixTheDryProducts()
        {
            await Task.Delay(2000);
            Console.WriteLine("Step 5: Mix The Dry Products - Done!");
        }
        public static async Task GetMilk()
        {
            await Task.Delay(1000);
            Console.WriteLine("Step 6: Get Milk - Done!");
        }
        public static async Task GetEggs()
        {
            await Task.Delay(1000);
            Console.WriteLine("Step 7: Get Eggs - Done!");
        }
        public static async Task GetOil()
        {
            await Task.Delay(1000);
            Console.WriteLine("Step 8: Get Oil - Done!");
        }
        public static async Task MixTheLiquidProducts()
        {
            await Task.Delay(2000);
            Console.WriteLine("Step 9: Mix The Liquid Products - Done!");
        }
        public static async Task MixAllProducts()
        {
            await Task.Delay(2000);
            Console.WriteLine("Step 10: Mix All Products - Done!");
        }

        public static async Task Baking()
        {
            await Task.Delay(10000);
            Console.WriteLine("Step 11: Baking - Done!");
        }
    }
}
