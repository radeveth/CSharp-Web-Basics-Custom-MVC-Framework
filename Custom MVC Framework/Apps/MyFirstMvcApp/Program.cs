namespace MyFirstMvcApp
{

    using System.Collections.Generic;

    using SUS.HHTP;
    using SUS.MvcFramework;
    using System.Threading.Tasks;
    using MyFirstMvcApp.Controllers;

    public class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateHostAsync(new StartUp(), 80);
        }
    }
}