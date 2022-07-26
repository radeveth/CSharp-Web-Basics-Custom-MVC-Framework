namespace MyFirstMvcApp
{
    using System;

    using SUS.HTTP;
    using System.Net;
    using System.Net.Sockets;
    using SUS.HTTP.Contarcts;
    using System.Threading.Tasks;
    using System.Text;
    using System.IO;

    public class StartUp
    {
        static async Task Main(string[] args)
        {
            IHttpServer server = new HttpServer();
            server.AddRoute("/", HomePage);
            server.AddRoute("/rado", (request) => new HttpResponse("text/html", new byte[] { 48, 65, 6, 6, 6, 20, 66, 72, 6, 6, 20, 52, 61, 64, 6, 21 })); // random bytes :)
            server.AddRoute("/favicon.ico", Favicon);
            server.AddRoute("/about", About);
            server.AddRoute("/users/login", Login);
            await server.StartAsync(80);
        }
        public static HttpResponse HomePage(HttpRequest request)
        {
            string responseHtml = "<h1>Welcome!</h1>";
            byte[] responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }
        public static HttpResponse Favicon(HttpRequest request)
        {
            byte[] faviconBytes = File.ReadAllBytes(@"..\..\..\wwwroot\favicon.ico");
            HttpResponse response = new HttpResponse("image/vnd.microsoft.icon", faviconBytes);

            return response;
        }
        public static HttpResponse About(HttpRequest request)
        {
            string responseHtml = "<h1>About...</h1>";
            byte[] responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }

        public static HttpResponse Login(HttpRequest request)
        {
            string responseHtml = "<h1>Login...</h1>";
            byte[] responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }
    }
}