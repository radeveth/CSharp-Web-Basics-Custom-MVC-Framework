namespace MyFirstMvcApp.Controllers
{
    using System;
    using System.IO;

    using SUS.HTTP;
    using SUS.MvcFramework;

    public class StaticFilesController : Controller
    {
        public HttpResponse Favicon(HttpRequest request)
        {
            return this.File(@"wwwroot\img\favicon.ico", "image/vnd.microsoft.icon");
        }

        public HttpResponse BootstrapCss(HttpRequest arg)
        {
            return this.File(@"wwwroot\css\bootstrap.min.css", "text/css");
        }

        public HttpResponse CustomCss(HttpRequest arg)
        {
            return this.File(@"wwwroot\css\custom.css", "text/css");
        }

        public HttpResponse BootstrapJs(HttpRequest arg)
        {
            return this.File(@"wwwroot\js\bootstrap.bundle.min.js", "application/javascript");
        }

        public HttpResponse CustomJs(HttpRequest arg)
        {
            return this.File(@"wwwroot\js\custom.js", "application/javascript");
        }
    }
}
