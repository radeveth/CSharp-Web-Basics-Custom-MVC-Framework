namespace SUS.MvcFramework.ViewEngine
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    
    using SUS.MvcFramework.ViewEngine.Contracts;

    public class ErrorView : IView
    {
        private readonly IEnumerable<string> errors;
        private readonly string csharpCode;

        public ErrorView(IEnumerable<string> errors, string csharpCode)
        {
            this.errors = errors;
            this.csharpCode = csharpCode;
        }

        public string ExecuteTemplate(object viewModel)
        {
            StringBuilder htmlErrorsBuilder = new StringBuilder();

            htmlErrorsBuilder.AppendLine($"<h1>View compile {this.errors.Count()} errors: </h1> <ul>");
            foreach (var error in this.errors)
            {
                htmlErrorsBuilder.AppendLine($"<li>{error}</li>");
            }

            htmlErrorsBuilder.AppendLine($"<ul><pre>{csharpCode}</pre>");

            return htmlErrorsBuilder.ToString();
        }
    }
}