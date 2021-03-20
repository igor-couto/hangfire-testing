using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            };
            var stopwatch = new Stopwatch();

            Console.WriteLine("Starting requests:\n");

            while(true)
            {
                stopwatch.Start();
                var response = await httpClient.PostAsync("fireandforget/test", default);
                stopwatch.Stop();

                Console.WriteLine($"Request result: \n{response.StatusCode} - {response.ReasonPhrase} \nTime: {stopwatch.Elapsed.TotalMilliseconds} milliseconds\n\n");
                stopwatch.Reset();
            }
        }
    }
}
