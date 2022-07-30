using SUS.HTTP.Enums;

namespace SUS.MvcFramework.Attribute
{
    public class HttpPostAttribute : BaseHttpAttribute
    {
        public HttpPostAttribute()
        {
        }
        public HttpPostAttribute(string url)
        {

        }

        public override HttpMethod Method => HttpMethod.Post;
    }
}
