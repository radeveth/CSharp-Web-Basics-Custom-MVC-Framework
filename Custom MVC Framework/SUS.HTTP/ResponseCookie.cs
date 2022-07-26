namespace SUS.HTTP
{

    using System.Text;
    
    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string name, string value) 
            : base(name, value)
        {
            this.Path = "/";
        }

        // Domain, Secure, Expires...
        public string Path { get; set; }
        public bool HttpOnly { get; set; }
        public int MaxAge { get; set; }

        public override string ToString()
        {
            StringBuilder cookieBuilder = new StringBuilder();
            cookieBuilder.Append($"{this.Name}={this.Value};");

            //if (!string.IsNullOrWhiteSpace(this.Path))
            //{
            //    cookieBuilder.Append($" Path={this.HttpOnly}");
            //}
            if (this.MaxAge != 0)
            {
                cookieBuilder.Append($" Max-Age={this.MaxAge};");
            }
            if (this.HttpOnly)
            {
                cookieBuilder.Append($" HttpOnly;");
            }

            return cookieBuilder.ToString();
        }
    }
}
