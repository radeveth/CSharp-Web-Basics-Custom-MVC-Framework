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
                return;
            }

            routeTable.Add(path, action);
            //else
            //{
            //    routeTable.Add(path, action);
            //}
        }

        public async Task StartAsync(int port = 80)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();

            TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
            ProcessClientAsync(tcpClient);
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

                Console.WriteLine(requestString);
                Console.WriteLine(new String('-', 80));

                string responseHtml = "<h1>Welcome!</h1>";
                byte[] responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);

                string responseString = $"HTTP/1.1 200 OK {HttpConstants.NewLine}" +
                                        $"Server: SUS Server 1.0 {HttpConstants.NewLine}" +
                                        $"Content-Type: text/html charset=utf-8 {HttpConstants.NewLine}" +
                                        $"Content-Length: {responseBodyBytes.Length}" +
                                        $"{HttpConstants.NewLine}" +
                                        $"{HttpConstants.NewLine}";

                byte[] responseHeadersBytes = Encoding.UTF8.GetBytes(responseString);
                await stream.WriteAsync(responseHeadersBytes, 0, responseHeadersBytes.Length);
                await stream.WriteAsync(responseBodyBytes, 0, responseBodyBytes.Length);

                Console.WriteLine(responseString);
                Console.WriteLine(responseHtml);
                Console.WriteLine(new String('=', 80));

                //await stream.WriteAsync();

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
