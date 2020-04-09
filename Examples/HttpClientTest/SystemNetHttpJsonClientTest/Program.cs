using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Common;

namespace SystemNetHttpJsonClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient {BaseAddress = new Uri(Constant.ApiUri)};

            var result = await client.GetFromJsonAsync<Data>(Constant.UsersResource);
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            Console.WriteLine(JsonSerializer.Serialize(result, options));
            Console.ReadKey();
        }
    }
}
