namespace MyFirstMvcApp
{

    using System.Collections.Generic;

    using SUS.HHTP;
    using SUS.MvcFramework;
    using System.Threading.Tasks;
    using MyFirstMvcApp.Controllers;

    public class StartUp
    {
        static async Task Main(string[] args)
        {
            List<Route> routeTable = new List<Route>();

            routeTable.Add(new Route("/", new HomeController().Index));
            routeTable.Add(new Route("/favicon.ico", new StaticFilesController().Favicon));
            routeTable.Add(new Route("/users/login", new UsersController().Login));
            routeTable.Add(new Route("/users/register", new UsersController().Register));
            routeTable.Add(new Route("/cards/add", new StaticFilesController().Add));
            routeTable.Add(new Route("/cards/all", new StaticFilesController().All));
            routeTable.Add(new Route("/cards/collection", new StaticFilesController().Collection)); 
            
            routeTable.Add(new Route("/css/bootstrap.min.css", new StaticFilesController().BootstrapCss));
            routeTable.Add(new Route("/css/custom.css", new StaticFilesController().CustomCss));
            routeTable.Add(new Route("/js/bootstrap.bundle.min.js", new StaticFilesController().BootstrapJs));
            routeTable.Add(new Route("/js/custom.js", new StaticFilesController().CustomJs));

            await Host.CreateHostAsync(routeTable, 80);
        }
    }
}