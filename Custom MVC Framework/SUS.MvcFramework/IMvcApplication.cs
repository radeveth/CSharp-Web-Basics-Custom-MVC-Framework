namespace SUS.MvcFramework
{

    using SUS.HHTP;
    using System.Collections.Generic;

    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(List<Route> routeTable);
    }
}
