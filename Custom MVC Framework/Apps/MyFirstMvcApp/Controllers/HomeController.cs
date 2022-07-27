namespace MyFirstMvcApp.Controllers
{

    using System.Text;

    using SUS.HTTP;
    using SUS.MvcFramework;

    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            return this.View();
        }
    }
}
