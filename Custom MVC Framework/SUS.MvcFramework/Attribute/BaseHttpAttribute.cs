namespace SUS.MvcFramework.Attribute
{
    using System;

    public abstract class BaseHttpAttribute : Attribute
    {
        public string Url { get; set; }
        public abstract SUS.HTTP.Enums.HttpMethod Method { get; }
    }
}
