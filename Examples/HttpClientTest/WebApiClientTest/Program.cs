using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Common;

namespace WebApiClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient {BaseAddress = new Uri(Constant.ApiUri)};
            var response = await client.GetAsync(Constant.UsersResource);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<Data>();
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
            Console.ReadKey();
        }
    }
}
