namespace SUS.MvcFramework.ViewEngine
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using System.IO;
    using System.Reflection;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Emit;
    using Microsoft.CodeAnalysis.CSharp;

    using SUS.MvcFramework.ViewEngine.Contracts;
    using SUS.MvcFramework.ViewlEngine.Contarcts;

    public class SusViewEngine : IViewEngine
    {
        // template code (c# + html) + view model
        // remove @ from template code where have csharp code
        // get clean csharp code as string -> string -> roslyn -> executable csharp code
        // return clean html as string

        public string GetHtml(string temlateCode, object viewModel)
        {
            string csharpCode = GenerateCSharpFromTemplate(temlateCode, viewModel);
            IView executableObject = GenerateExecutableCоde(csharpCode, viewModel);
            string html = executableObject.ExecuteTemplate(viewModel);

            return html;
        }


        private string GenerateCSharpFromTemplate(string templateCode, object viewModel)
        {
            string typeOfModel = "object";
            if (viewModel != null)
            {
                if (viewModel.GetType().IsGenericType)
                {
                    var modelName = viewModel.GetType().FullName;
                    var genericArguments = viewModel.GetType().GenericTypeArguments;
                    typeOfModel = modelName.Substring(0, modelName.IndexOf('`'))
                        + "<" + string.Join(",", genericArguments.Select(x => x.FullName)) + ">";
                }
                else
                {
                    typeOfModel = viewModel.GetType().FullName;
                }
            }

            string csharpCode = @"
            using System;
            using System.Linq;
            using System.Text;
            using System.Collections.Generic;
            using SUS.MvcFramework.ViewEngine.Contracts;

            namespace ViewNamespace
            {
                public class ViewClass : IView
                {
                    public string ExecuteTemplate(object viewModel)
                    {
                        var Model = viewModel as " + typeOfModel + @"
                        var html = new StringBuilder();
                        " + GetMethodBody(templateCode) + @"

                        return html.ToString();
                    }
                }
            }";


            return csharpCode;
        }

        private string GetMethodBody(string templateCode)
        {
            Regex csharpCodeRegex = new Regex(@"[^\""\s&\'\<]+");
            var supportedOperators = new List<string> { "for", "while", "if", "else", "foreach" };
            StringBuilder csharpCode = new StringBuilder();
            StringReader sr = new StringReader(templateCode);


            // Example: 
            // line = <li>@i<span>@(i+1)</span></li>
            // variable - charpCode = html.AppendLine(@"
            // when we have @ => before @ we have clean html and after @ we have clean csharp
            // first append html code => variable - charpCode = html.AppendLine(@"<li>" +
            // after that we find where csharp code finish (regex) and append to -
            //                         charpCode = html.AppendLine(@"<li>" + i + "
            // remove the part of line - <span>@(i+1)</span></li>
            // again find where have @ symbol and get clean html code and clean csharp code after that -
            //                       charpCode = html.AppendLine(@"<li>" + i + "<span>" + (i+1) + "
            // remove the part of line - </span></li>
            // .... repeat tothe end of line ....

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (supportedOperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    var atSignLocation = line.IndexOf("@");
                    line = line.Remove(atSignLocation, 1);
                    csharpCode.AppendLine(line);
                }
                else if (line.TrimStart().StartsWith("{") ||
                    line.TrimStart().StartsWith("}"))
                {
                    csharpCode.AppendLine(line);
                }
                else
                {
                    csharpCode.Append($"html.AppendLine(@\"");

                    while (line.Contains("@"))
                    {
                        var atSignLocation = line.IndexOf("@");
                        var htmlBeforeAtSign = line.Substring(0, atSignLocation);
                        csharpCode.Append(htmlBeforeAtSign.Replace("\"", "\"\"") + "\" + ");
                        var lineAfterAtSign = line.Substring(atSignLocation + 1);
                        var code = csharpCodeRegex.Match(lineAfterAtSign).Value;
                        csharpCode.Append(code + " + @\"");
                        line = lineAfterAtSign.Substring(code.Length);
                    }

                    csharpCode.AppendLine(line.Replace("\"", "\"\"") + "\");");
                }
            }

            return csharpCode.ToString();
        }

        private IView GenerateExecutableCоde(string csharpCode, object viewModel)
        {
            var compileResult = CSharpCompilation.Create("ViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));
            if (viewModel != null)
            {
                if (viewModel.GetType().IsGenericType)
                {
                    var genericArguments = viewModel.GetType().GenericTypeArguments;
                    foreach (var genericArgument in genericArguments)
                    {
                        compileResult = compileResult
                            .AddReferences(MetadataReference.CreateFromFile(genericArgument.Assembly.Location));
                    }
                }

                compileResult = compileResult
                    .AddReferences(MetadataReference.CreateFromFile(viewModel.GetType().Assembly.Location));
            }

            var libraries = Assembly.Load(
                new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var library in libraries)
            {
                compileResult = compileResult
                    .AddReferences(MetadataReference.CreateFromFile(
                        Assembly.Load(library).Location));
            }

            compileResult = compileResult.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(csharpCode));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                EmitResult result = compileResult.Emit(memoryStream);
                if (!result.Success)
                {
                    return new ErrorView(result.Diagnostics
                        .Where(x => x.Severity == DiagnosticSeverity.Error)
                        .Select(x => x.GetMessage()), csharpCode);
                }

                try
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var byteAssembly = memoryStream.ToArray();
                    var assembly = Assembly.Load(byteAssembly);
                    var viewType = assembly.GetType("ViewNamespace.ViewClass");
                    var instance = Activator.CreateInstance(viewType);
                    return (instance as IView)
                        ?? new ErrorView(new List<string> { "Instance is null!" }, csharpCode);
                }
                catch (Exception ex)
                {
                    return new ErrorView(new List<string> { ex.ToString() }, csharpCode);
                }
            }
        }
    }
}