namespace SUS.HTTP
{

    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    
    using System.Net.Http;

    public class HttpRequest
    {
        private HttpRequest()
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();
        }

        public HttpRequest(string requestString)
            : this()
        {
            string[] lines = requestString
                .Split(new string[] { HttpConstants.NewLine }, StringSplitOptions.None).ToArray();

            string headerLine = lines[0];
            string[] headerLineParts = headerLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            this.Path = headerLineParts[1];


            bool isInHeaders = true;
            StringBuilder bodyBuilder = new StringBuilder();

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if (String.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;
                    continue;
                }

                if (isInHeaders)
                {
                    this.Headers.Add(new Header(line));
                }
                else if (!isInHeaders)
                {
                    bodyBuilder.AppendLine(line);
                }

            }

            if (this.Headers.Any(h => h.Name == HttpConstants.RequestCookieHeader))
            {
                string[] cookies = this.Headers
                    .FirstOrDefault(h => h.Name == HttpConstants.RequestCookieHeader)
                    .Value.ToString()
                    .Split("; ", StringSplitOptions.RemoveEmptyEntries);

                foreach (string cookie in cookies)
                {
                    this.Cookies.Add(new Cookie(cookie));
                }
            }

            this.Body = bodyBuilder.ToString();
        }

        public string Path { get; set; }
        public HttpMethod Method { get; set; }
        public ICollection<Header> Headers { get; set; }
        public ICollection<Cookie> Cookies { get; set; }
        public string Body { get; set; }
    }
}
