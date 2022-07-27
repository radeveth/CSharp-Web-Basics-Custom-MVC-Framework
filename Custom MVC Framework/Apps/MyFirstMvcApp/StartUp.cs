namespace MyFirstMvcApp
{
    using MyFirstMvcApp.Controllers;
    using SUS.HHTP;
    using SUS.MvcFramework;
    using System.Collections.Generic;
    using System.Net.Http;

    public class StartUp : IMvcApplication
    {

        public void ConfigureServices()
        {

        }
        public void Configure(List<Route> routeTable)
        {
            routeTable.Add(new Route("/", SUS.HTTP.Enums.HttpMethod.GET, new HomeController().Index));
            routeTable.Add(new Route("/users/login", SUS.HTTP.Enums.HttpMethod.GET, new UsersController().Login));
            routeTable.Add(new Route("/users/login", SUS.HTTP.Enums.HttpMethod.Post, new UsersController().DoLogin));
            routeTable.Add(new Route("/users/register", SUS.HTTP.Enums.HttpMethod.GET, new UsersController().Register));
            routeTable.Add(new Route("/cards/add", SUS.HTTP.Enums.HttpMethod.GET, new CardsController().Add));
            routeTable.Add(new Route("/cards/all", SUS.HTTP.Enums.HttpMethod.GET, new CardsController().All));
            routeTable.Add(new Route("/cards/collection", SUS.HTTP.Enums.HttpMethod.GET, new CardsController().Collection));

            routeTable.Add(new Route("/favicon.ico", SUS.HTTP.Enums.HttpMethod.GET, new StaticFilesController().Favicon));
            routeTable.Add(new Route("/css/bootstrap.min.css", SUS.HTTP.Enums.HttpMethod.GET, new StaticFilesController().BootstrapCss));
            routeTable.Add(new Route("/css/custom.css", SUS.HTTP.Enums.HttpMethod.GET, new StaticFilesController().CustomCss));
            routeTable.Add(new Route("/js/bootstrap.bundle.min.js", SUS.HTTP.Enums.HttpMethod.GET, new StaticFilesController().BootstrapJs));
            routeTable.Add(new Route("/js/custom.js", SUS.HTTP.Enums.HttpMethod.GET, new StaticFilesController().CustomJs));
        }
    }
}
