using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using Refit;

namespace RefitClientTest
{
    //https://github.com/reactiveui/refit

    public interface IRestApi
    {
        [Get("/api/users")]
        Task<Data> GetUser();
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var apiClient = RestService.For<IRestApi>(Constant.ApiUri);
            var result = await apiClient.GetUser();
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
            Console.ReadKey();
        }
    }
}
