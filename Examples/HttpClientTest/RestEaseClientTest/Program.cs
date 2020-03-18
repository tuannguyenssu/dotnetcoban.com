using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using RestEase;

namespace RestEaseClientTest
{
    //https://github.com/canton7/RestEase
    public interface IRestApi
    {
        [Get("/api/users")]
        Task<Data> GetUser();
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var apiClient = RestClient.For<IRestApi>(Constant.ApiUri);
            var result = await apiClient.GetUser();
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
            Console.ReadKey();
        }
    }
}
