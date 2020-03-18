using System;
using System.Net;
using System.Threading.Tasks;
using Common;

namespace WebClientWebRequestTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var request = WebRequest.Create($"{Constant.ApiUri}{Constant.UsersResource}");
            //request.Method = "GET";

            //var response = await request.GetResponseAsync();
            //using var stream = response.GetResponseStream();
            //var result = await JsonSerializer.DeserializeAsync<Data>(stream);
            //Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            //{
            //    WriteIndented = true
            //}));

            //Console.ReadKey();

            var client = new WebClient();
            var result = await client.DownloadStringTaskAsync($"{Constant.ApiUri}{Constant.UsersResource}");
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
