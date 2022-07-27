namespace SUS.HTTP.Contarcts
{

    using System;

    using System.Threading.Tasks;
    

    public interface IHttpServer
    {
        Task StartAsync(int port = 80);
    }
}