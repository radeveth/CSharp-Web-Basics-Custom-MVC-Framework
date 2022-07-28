namespace SUS.MvcFramework.ViewEngine
{
    using System;
    using System.Linq;
    
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
            string csharpCode = GenerateCSharpFromTemplate(temlateCode);
            IView executableObject = GenerateExecutableCode(csharpCode, viewModel);
            string html = executableObject.ExecuteTemplate(viewModel);

            return html;
        }


        private string GenerateCSharpFromTemplate(string temlateCode)
        {
            string methodBody = GetMethodBody(temlateCode);

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
                        var html = new StringBuilder();

                        " + methodBody + @"

                        return html.ToString();
                    }
                }
            }   
            ";


            return csharpCode;
        }

        private string GetMethodBody(string temlateCode)
        {
            return String.Empty;
        }

        private IView GenerateExecutableCode(string csharpCode, object viewModel)
        {
            // Roslyn
            // C# -> executable -> IView -> ExecuteTemplate

            var compileResult = CSharpCompilation.Create("ViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));

            if (viewModel != null)
            {
                compileResult = compileResult
                    .AddReferences(MetadataReference.CreateFromFile(viewModel.GetType().Assembly.Location));

                var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

                foreach (var library in libraries)
                {
                    compileResult = compileResult
                        .AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
                }
            }

            compileResult = compileResult.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(csharpCode));

            using MemoryStream stream = new MemoryStream();
            EmitResult result = compileResult.Emit(stream);

            if (!result.Success)
            {
                return new ErrorView(result.Diagnostics
                    .Where(e => e.Severity == DiagnosticSeverity.Error)
                    .Select(e => e.ToString()), csharpCode);
            }

            stream.Seek(0, SeekOrigin.Begin);
            byte[] byteAssembly = stream.ToArray();
            Assembly assembly = Assembly.Load(byteAssembly);
            Type viewType = assembly.GetType("ViewNamespace.ViewClass");
            // object instance = Activator.CreateInstance(viewType);
            var instance = Activator.CreateInstance(viewType);

            return instance as IView;
        }
    }
}
