namespace MyFirstMvcApp
{
    using SUS.HTTP;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class StartUp
    {
        static async Task Main(string[] args)
        {
            HttpServer server = new HttpServer();
            server.AddRoute("/", HomePage);
            server.AddRoute("/favicon.ico", Favicon);
            server.AddRoute("/about", About);
            server.AddRoute("/users/login", Login);
            await server.StartAsync();
        }

        private static HttpResponse Login(HttpRequest arg)
        {
            throw new NotImplementedException();
        }

        private static HttpResponse About(HttpRequest arg)
        {
            throw new NotImplementedException();
        }

        private static HttpResponse Favicon(HttpRequest arg)
        {
            throw new NotImplementedException();
        }

        private static HttpResponse HomePage(HttpRequest arg)
        {
            throw new NotImplementedException();
        }
    }
}
