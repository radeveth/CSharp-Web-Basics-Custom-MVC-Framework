namespace MyFirstMvcApp.Controllers
{
    using System;
    using System.IO;
    using System.Text;

    using SUS.HTTP;
    using SUS.MvcFramework;

    public class UsersController : Controller
    {
        public HttpResponse Login(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse Register(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse DoLogin(HttpRequest request)
        {
            // TODO: read data
            // TODO: check user
            // TODO: log user
            return this.Redirect("/");
        }
    }
}
