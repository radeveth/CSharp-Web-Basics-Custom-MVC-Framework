namespace SUS.MvcFramework
{
    using SUS.HHTP;
    using SUS.HTTP;
    using System.IO;
    using System.Linq;
    using SUS.HTTP.Contarcts;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Reflection;
    using SUS.MvcFramework.Attribute;
    using System;

    public class Host
    {
        public Host()
        {
        }

        public static async Task CreateHostAsync(IMvcApplication application, int port = 80)
        {
            List<Route> routeTable = new List<Route>();
            application.ConfigureServices();
            application.Configure(routeTable);

            IHttpServer server = new HttpServer(routeTable);
            await server.StartAsync(port);

            AutoRegisterStaticFiles(routeTable);
            AutoRegisterRoutes(routeTable, application);

            System.Console.WriteLine("All Regstered routes:");
            foreach (var route in routeTable)
            {
                System.Console.WriteLine(route.Path);
            }
        }

        private static void AutoRegisterRoutes(List<Route> routeTable, IMvcApplication application)
        {
            var controllerTypes = application.GetType().Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Controller)));

            foreach (var controllerType in controllerTypes)
            {
                List<MethodInfo> methods = (List<MethodInfo>)controllerType.GetMethods()
                    .Where(x => x.IsPublic && !x.IsStatic && x.DeclaringType == controllerType
                    && !x.IsAbstract && !x.IsConstructor && !x.IsSpecialName);

                foreach (var method in methods)
                {
                    string url = $"/{(controllerType.Name.ToString()).Replace("Controller", string.Empty)}/{method.Name}";

                    BaseHttpAttribute attribute = (BaseHttpAttribute)method.GetCustomAttributes(false)
                        .Where(x => x.GetType().IsSubclassOf(typeof(BaseHttpAttribute)))
                        .FirstOrDefault();

                    var httpMethod = SUS.HTTP.Enums.HttpMethod.GET;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (!string.IsNullOrEmpty(attribute.Url))
                    {
                        url = attribute.Url;
                    }

                    routeTable.Add(new Route(url, httpMethod, (request) =>
                    {
                        var instance = (Controller)Activator.CreateInstance(controllerType);
                        instance.Request = request;
                        var response = method.Invoke(instance, new object[] { }) as HttpResponse;

                        return response;
                    }));
                }
            }
        }

        private static void AutoRegisterStaticFiles(List<Route> routeTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);

            foreach (var staticFile in staticFiles)
            {
                var url = staticFile.Replace("wwwroot", string.Empty)
                    .Replace("\\", "/");
                routeTable.Add(new Route(url, SUS.HTTP.Enums.HttpMethod.GET, (request) =>
                {
                    var fileContent = File.ReadAllBytes(staticFile);
                    var fileExt = new FileInfo(staticFile).Extension;
                    var contentType = fileExt switch
                    {
                        ".txt" => "text/plain",
                        ".js" => "text/javascript",
                        ".css" => "text/css",
                        ".jpg" => "image/jpg",
                        ".jpeg" => "image/jpg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".ico" => "image/vnd.microsoft.icon",
                        ".html" => "text/html",
                        _ => "text/plain",
                    };

                    return new HttpResponse(contentType, fileContent, SUS.HTTP.Enums.HttpStatusCode.Ok);
                }));
            }
        }
    }
}
