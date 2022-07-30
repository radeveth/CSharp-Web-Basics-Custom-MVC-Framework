namespace MyFirstMvcApp.Controllers
{

    using System.Text;

    using SUS.HTTP;
    using SUS.MvcFramework;
    using SUS.MvcFramework.Attribute;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            return this.View();
        }
    }
}
