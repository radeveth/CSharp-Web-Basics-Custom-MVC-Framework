namespace SUS.MvcFramework.ViewlEngine.Contarcts
{
    public interface IViewEngine
    {
        string GetHtml(string temlateCode, object viewModel);
    }
}
