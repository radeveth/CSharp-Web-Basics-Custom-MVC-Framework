namespace SUS.MvcFramework
{

    using System.Text;

    using SUS.HTTP;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class Controller
    {
        public HttpResponse View([CallerMemberName] string methodName = null)
        {
            string layout = System.IO.File.ReadAllText(@"Views\Shared\_Layout.html");

            // Views\Users\Login.html
            string responseHtml = layout
                .Replace("@RenderBody()", System.IO.File.ReadAllText
                        (@$"Views\{this.GetType().Name.Replace("Controller", string.Empty)}\{methodName}.html")); 
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
