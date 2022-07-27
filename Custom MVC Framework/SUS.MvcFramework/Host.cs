namespace SUS.MvcFramework
{
    using SUS.HHTP;
    using SUS.HTTP;
    using SUS.HTTP.Contarcts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Host
    {
        public Host()
        {
        }

        public static async Task CreateHostAsync(List<Route> routeTable, int port = 80)
        {
            IHttpServer server = new HttpServer(routeTable);

            await server.StartAsync(port);
        }
    }
}
