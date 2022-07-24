using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpDemo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var sites = new List<string>()
            {
                "https://softuni.bg",
                "https://creative.softuni.bg",
                "https://digital.softuni.bg"
            };

            var sw = Stopwatch.StartNew();

            File.WriteAllText(@"..\..\..\softuni.html", await DownloadContentSiteAsync(sites[0]));
            File.WriteAllText(@"..\..\..\creative.html", await DownloadContentSiteAsync(sites[1]));
            File.WriteAllText(@"..\..\..\digital.html", await DownloadContentSiteAsync(sites[2]));

            sw.Stop();
            Console.WriteLine(sw.Elapsed.Milliseconds);
        }

        private static async Task<string> DownloadContentSiteAsync(string url)
        {
            using var httpClient = new HttpClient();
            using var webSiteTask = await httpClient.GetAsync(url);

            httpClient.Dispose();
            webSiteTask.Dispose();
            return await webSiteTask.Content.ReadAsStringAsync();
        }
    }
}
