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
            var numberOfRequestsToBeDone = 50;

            if (args.Length > 0) 
                numberOfRequestsToBeDone = int.Parse(args[0]);

            var averageSyncTime = 0d;
            var averageAsyncTime = 0d;
            var averageHangfireTime = 0d;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            };
            var stopwatch = new Stopwatch();

            Console.WriteLine("Starting requests:\n");

            for(var i = 0; i < numberOfRequestsToBeDone; i++)
            {
                stopwatch.Start();
                var response = await httpClient.PostAsync("fireandforget/syncronous", default);
                stopwatch.Stop();

                Console.WriteLine($"Request result: \n\t{(int)response.StatusCode} - {response.ReasonPhrase} \n\tTime: {stopwatch.Elapsed.TotalMilliseconds} milliseconds\n");
                averageSyncTime += stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset();
            }
            averageSyncTime = averageSyncTime / numberOfRequestsToBeDone;

            for (var i = 0; i < numberOfRequestsToBeDone; i++)
            {
                stopwatch.Start();
                var response = await httpClient.PostAsync("fireandforget/asyncronous", default);
                stopwatch.Stop();

                Console.WriteLine($"Request result: \n\t{(int)response.StatusCode} - {response.ReasonPhrase} \n\tTime: {stopwatch.Elapsed.TotalMilliseconds} milliseconds\n");
                averageAsyncTime += stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset();
            }
            averageAsyncTime = averageAsyncTime / numberOfRequestsToBeDone;

            for (var i = 0; i < numberOfRequestsToBeDone; i++)
            {
                stopwatch.Start();
                var response = await httpClient.PostAsync("fireandforget/hangfire", default);
                stopwatch.Stop();

                Console.WriteLine($"Request result: \n\t{(int)response.StatusCode} - {response.ReasonPhrase} \n\tTime: {stopwatch.Elapsed.TotalMilliseconds} milliseconds\n");
                averageHangfireTime += stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset();
            }
            averageHangfireTime = averageHangfireTime / numberOfRequestsToBeDone;

            Console.WriteLine("TEST RESULT:");
            Console.WriteLine($"\tAverage Syncronous Time: {averageSyncTime}");
            Console.WriteLine($"\tAverage Asyncronous Time: {averageAsyncTime}");
            Console.WriteLine($"\tAverage Hangfire Time: {averageHangfireTime}");
        }
    }
}
