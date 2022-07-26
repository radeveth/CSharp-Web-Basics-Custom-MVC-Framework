namespace SUS.HTTP
{

    using System;
    using System.Text;
    using System.Collections.Generic;

    using System.Net;
    using System.Net.Sockets;
    using SUS.HTTP.Contarcts;
    using System.Threading.Tasks;

    public class HttpServer : IHttpServer
    {
        IDictionary<string, Func<HttpRequest, HttpResponse>> routeTable = 
            new Dictionary<string, Func<HttpRequest, HttpResponse>>();

        public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
        {
            if (routeTable.Keys.Contains(path))
            {
                routeTable[path] = action;
            }
            else
            {
                routeTable.Add(path, action);
            }
        }

        public async Task StartAsync(int port = 80)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                ProcessClientAsync(tcpClient);
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            try
            {
                using NetworkStream stream = tcpClient.GetStream();

                int positionInStream = 0;
                byte[] buffer = new byte[HttpConstants.BufferLength];
                List<byte> data = new List<byte>();

                while (true)
                {
                    int length = await stream.ReadAsync(buffer, positionInStream, buffer.Length);
                    positionInStream += length;

                    if (length < buffer.Length)
                    {
                        byte[] partialBuffer = new byte[length];
                        Array.Copy(buffer, partialBuffer, length);
                        data.AddRange(partialBuffer);
                        break;
                    }
                    else
                    {
                        data.AddRange(buffer);
                    }
                }

                // byte[] => string(text)
                // ASCII -> 1 byte = 1 symbol
                // Unicode -> 2 byte = 1 symbol
                // UTF8 -> Many bytes = 1 symbol
                string requestString = Encoding.UTF8.GetString(data.ToArray());
                HttpRequest request = new HttpRequest(requestString);
                HttpResponse response;

                if (this.routeTable.ContainsKey(request.Path))
                {
                    Func<HttpRequest, HttpResponse> action = this.routeTable[request.Path];
                    response = action(request);
                }
                else
                {
                    // Not Found 404
                    response = new HttpResponse("text/html", new byte[0], Enums.HttpStatusCode.NotFound);
                }

                response.Headers.Add(new Header("Server", "SUS Server 1.0"));
                response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString())
                { HttpOnly = true, MaxAge = 3 * 24 * 60 * 60 });
                byte[] responseHeaderBytes = Encoding.UTF8.GetBytes(response.ToString());

                await stream.WriteAsync(responseHeaderBytes, 0, responseHeaderBytes.Length);
                await stream.WriteAsync(response.Body, 0, response.Body.Length);

                Console.WriteLine($"Request => {request.Method}");
                Console.WriteLine($"Response => {(int)response.StatusCode} {response.StatusCode} - " +
                                  $"{Encoding.UTF8.GetString(response.Body, 0, response.Body.Length)}");
                Console.WriteLine(new String('=', 80));

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
