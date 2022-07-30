namespace SUS.MvcFramework.Attribute
{
    using System;
    using SUS.HTTP.Enums;
    public class HttpGetAttribute : BaseHttpAttribute
    {
        public HttpGetAttribute()
        {
        }
        public HttpGetAttribute(string url)
        {

        }

        public override HttpMethod Method => HttpMethod.GET;
    }
}
