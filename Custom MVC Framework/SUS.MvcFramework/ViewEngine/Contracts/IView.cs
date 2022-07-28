namespace SUS.MvcFramework.ViewEngine.Contracts
{
    public interface IView
    {
        string ExecuteTemplate(object viewModel);
    }
}
