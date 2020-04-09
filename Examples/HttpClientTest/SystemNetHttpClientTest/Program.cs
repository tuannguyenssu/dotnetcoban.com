using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Common;

namespace SystemNetHttpClientTest
{
    //https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient {BaseAddress = new Uri(Constant.ApiUri)};

            //var response = await client.GetStringAsync(Constant.UsersResource);
            //var result = JsonSerializer.Deserialize<Data>(response);
            //Console.WriteLine(result);

            var response = await client.GetStreamAsync(Constant.UsersResource);
            var result = await JsonSerializer.DeserializeAsync<Data>(response);
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));

            Console.ReadKey();
        }
    }
}
