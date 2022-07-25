using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientDemo
{
    public class Program
    {
        public static Dictionary<string, int> SessionStorage = new Dictionary<string, int>();
        private const string NewLine = "\r\n";
        public static async Task Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 12345);
            await Program.HttpClient(tcpListener);
        }

        private static async Task HttpClient(TcpListener tcpListener)
        {
            if (tcpListener == null)
            {
                // Create instance of new TcpListener with parameters: IPAddress (localhost), port
                tcpListener = new TcpListener(IPAddress.Loopback, 12345);
            }

            tcpListener.Start(); // Starting the connection

            while (true)
            {
                // Accept connection from client
                TcpClient client = tcpListener.AcceptTcpClient();
                // Open the stream
                using NetworkStream stream = client.GetStream();
                


                // Create array of bytes
                byte[] buffer = new byte[1000000];
                int length = await stream.ReadAsync(buffer, 0, buffer.Length);
                // Get request from client like string
                string requestString = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(requestString);
                Console.WriteLine(new string('=', 80));

                // Create Identifier like Guid
                string sid = Guid.NewGuid().ToString();
                Match match = Regex.Match(requestString, @"sid=[^\n]*\r\n");
                if (match.Success)
                {
                    sid = match.Value.Substring(4);
                }

                if (!SessionStorage.Keys.Contains(sid))
                {
                    SessionStorage[sid] = 0;
                }
                SessionStorage[sid]++;

                // Create content for response
                string html = $"<h1>Hello from RadoServer2022 for the {SessionStorage[sid]} time! </br> Time now: {DateTime.Now}!</h1>";

                // Create response like string
                string responseString = $"HTTP/1.1 200 OK {NewLine}" +
                                  $"Server: RadoServer 2022 {NewLine}" +
                                  //$"Location: https://www.google.com {NewLine}" +
                                  $"Content-Type: text/html {NewLine}" +
                                  $"Set-Cookie: sid={sid}; HttpOnly; Secure; Epires={DateTime.Now.AddMinutes(30).ToString("R")}; {NewLine}" +
                                  //$"Set-Cookie: _auth=1234567890qwertyuiopasdfghjklzxcvbnm; Domain=localhost:12345/; Path=/; Max-Age=1440;" +
                                  //$"Content-Disposition: attachment; filename=rado.txt {NewLine}" +
                                  //inline is default for content disposition
                                  //$"Content-Length: {html.Length} {NewLine}" +
                                  $"{NewLine}" +
                                  $"{html}" +
                                  $"{NewLine}";

                // Convert the response to array of bytes
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

                Console.WriteLine(responseString);
                Console.WriteLine(new string('=', 80));
                Console.WriteLine(new string('=', 80));
            }
        }

        private static async Task ReadData(string url)
        {
            HttpClient httpClient = new HttpClient();
            var website = await httpClient.GetAsync(url);
            //var html = await website.Content.ReadAsStringAsync();
            var statusCode = website.StatusCode;
            var headers = website.Headers.Select(x => new { x.Key, x.Value});

            foreach (var header in headers)
            {
                Console.WriteLine(header.Key);
                foreach (var value in header.Value)
                {
                    Console.WriteLine(value);
                }
                Console.WriteLine();
            }
        }
    }
}