namespace SUS.MvcFramework
{

    using System.Text;

    using SUS.HTTP;
    using System.Runtime.CompilerServices;
    using SUS.MvcFramework.ViewEngine;

    public abstract class Controller
    {
        private SusViewEngine viewEngine;
        public Controller()
        {
            this.viewEngine = new SusViewEngine();
        }

        public HttpRequest Request { get; set; }

        public HttpResponse View(object viewModel = null, [CallerMemberName] string methodName = null)
        {
            string layout = System.IO.File.ReadAllText(@"Views\Shared\_Layout.html");
            layout = layout.Replace("@RenderBody)", "___VIEW_GOES_HERE___");
            layout = this.viewEngine.GetHtml(layout, viewModel);

            // Views\Users\Login.html
            string responseHtml = layout
                .Replace("@RenderBody()", System.IO.File.ReadAllText
                        (@$"Views\{this.GetType().Name.Replace("Controller", string.Empty)}\{methodName}.html"));

            responseHtml = this.viewEngine.GetHtml(responseHtml, viewModel);


            byte[] responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }

        public HttpResponse File(string filePath, string contentType)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            HttpResponse response = new HttpResponse(contentType, fileBytes);

            return response;
        }

        public HttpResponse Redirect(string url)
        {
            HttpResponse response = new HttpResponse(HTTP.Enums.HttpStatusCode.Found);
            response.Headers.Add(new Header("Location", url));

            return response;
        }
    }
}
