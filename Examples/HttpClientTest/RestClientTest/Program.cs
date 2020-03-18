using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using RestClient.Net;

namespace RestClientTest
{
    //https://github.com/MelbourneDeveloper/RestClient.Net
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client(new Uri( Constant.ApiUri));
            var response = await client.GetAsync<Data>(Constant.UsersResource);
            var result = response.Body;
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
            Console.ReadKey();
        }
    }
}
