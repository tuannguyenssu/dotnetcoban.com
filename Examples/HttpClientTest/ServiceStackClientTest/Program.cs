using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using ServiceStack;

namespace ServiceStackClientTest
{
    //https://docs.servicestack.net/csharp-client
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new JsonServiceClient(Constant.ApiUri);
            var result = await client.GetAsync<Data>(Constant.UsersResource);
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
            Console.ReadKey();
        }
    }
}
