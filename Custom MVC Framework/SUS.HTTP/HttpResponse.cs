namespace SUS.HTTP
{
    using System.Text;
    using System.Collections.Generic;

    using SUS.HTTP.Enums;
    using SUS.HTTP.Contarcts;

    public class HttpResponse
    {
        public HttpResponse(string contentType, byte[] body, HttpStatusCode statusCode = HttpStatusCode.Ok)
        {
            this.StatusCode = statusCode;
            this.Body = body;
            this.Headers = new List<Header>()
            {
                new Header("Content-Type", contentType),
                new Header("Content-Length", this.Body.Length.ToString())
            };
            this.Cookies = new List<ResponseCookie>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public ICollection<Header> Headers { get; set; }
        public ICollection<ResponseCookie> Cookies { get; set; }
        public byte[] Body { get; set; }

        public override string ToString()
        {
            StringBuilder response = new StringBuilder();

            response.Append($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode.ToString()}" + HttpConstants.NewLine);
            foreach (Header header in this.Headers)
            {
                response.Append(header.ToString() + HttpConstants.NewLine);
            }

            foreach (Cookie cookie in this.Cookies)
            {
                response.Append($"Set-Cookie: {cookie}" + HttpConstants.NewLine);
            }

            response.Append(HttpConstants.NewLine);

            return response.ToString();
        }
    }
}
