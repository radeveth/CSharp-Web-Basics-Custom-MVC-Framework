namespace MyFirstMvcApp.Controllers
{
    using System;
    using System.IO;
    using System.Text;

    using SUS.HTTP;
    using SUS.MvcFramework;
    using SUS.MvcFramework.Attribute;

    public class UsersController : Controller
    {
        public HttpResponse Login()
        {
            return this.View();
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse DoLogin()
        {
            // TODO: read data
            // TODO: check user
            // TODO: log user
            return this.Redirect("/");
        }
    }
}
