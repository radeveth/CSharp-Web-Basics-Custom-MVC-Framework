namespace SUS.HHTP
{

    using System;
    using System.Net.Http;
    using SUS.HTTP;

    public class Route
    {
        public Route(string path, SUS.HTTP.Enums.HttpMethod method, Func<HttpRequest, HttpResponse> action)
        {
            this.Path = path;
            this.Method = method;
            this.Action = action;
        }

        public string Path { get; set; }
        public SUS.HTTP.Enums.HttpMethod Method { get; set; }
        public Func<HttpRequest, HttpResponse> Action { get; set; }
    }
}
