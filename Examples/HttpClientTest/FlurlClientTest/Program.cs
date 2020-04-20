using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using Flurl;
using Flurl.Http;

namespace FlurlClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = Constant.ApiUri;
            var result = await url.AppendPathSegment(Constant.UsersResource).GetAsync().ReceiveJson<Data>();

            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
            Console.ReadKey();
        }
    }
}
