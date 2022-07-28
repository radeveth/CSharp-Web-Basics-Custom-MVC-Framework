namespace SUS.MvcFramework.Tests
{
    using Xunit;
    using System;

    using SUS.MvcFramework.ViewEngine;
    using SUS.MvcFramework.ViewlEngine.Contarcts;
    using System.IO;

    public class SusViewEngineTests
    {
        // Process while unit testing
        // happy path
        // interesting cases
        // complex cases or combination of tests
        // code coverage 100%

        [Theory]
        [InlineData("CleanHtml")]
        [InlineData("Foreach")]
        [InlineData("IfElseFor")]
        [InlineData("ViewlModel")]
        public void GetHtmlMethodShouldReturnCorrectTextAsHtml(string fileName)
        {
            var viewlModel = new TestViewModel()
            {
                Name = "Doggo Arghention",
                DateOfBirth = new DateTime(2019, 6, 1),
                Price = 12345.67M
            };

            IViewEngine viewEngine = new SusViewEngine();
            string view = File.ReadAllText(@$"ViewTests\{fileName}.html");
            string result = viewEngine.GetHtml(view, viewlModel);
            string expected = File.ReadAllText(@$"ViewTests\{fileName}.Result.html");

            Assert.Equal(expected, result);
        }
    }
}
