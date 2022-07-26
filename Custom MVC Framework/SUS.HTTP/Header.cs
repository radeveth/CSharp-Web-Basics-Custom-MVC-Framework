namespace SUS.HTTP
{

    using System;

    public class Header
    {
        public Header(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public Header(string headerLine)
        {
            string[] headerParts = headerLine.Split(": ", 2, StringSplitOptions.None);
            this.Name = headerParts[0];
            this.Value = headerParts[1];
            
            //int index = headerLine.IndexOf(':');
            //this.Name = headerLine.Substring(0, index);
            //this.Value = headerLine.Substring(index + 1);
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{this.Name}: {this.Value}";
        }
    }
}