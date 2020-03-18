using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using RestSharp.Serializers.SystemTextJson;
using RestSharp.Serializers.Utf8Json;

namespace RestSharpClientTest
{
    //http://restsharp.org/
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new RestClient(Constant.ApiUri);
            client.UseUtf8Json();
            //client.UseSystemTextJson();
            //client.UseNewtonsoftJson();
            var request = new RestRequest(Constant.UsersResource, DataFormat.Json);

            var result = await client.GetAsync<Data>(request);
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
            Console.ReadKey();
        }
    }
}
