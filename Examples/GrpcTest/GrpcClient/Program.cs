using Grpc.Core;
using Grpc.Net.Client;
using GrpcUserEndpoint;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting GRPC Client...");
            Console.WriteLine();
            var httpClient = new HttpClient();
            string grpcServerUrl = "https://localhost:5001";
            httpClient.BaseAddress = new Uri(grpcServerUrl);
            var client = GrpcClient.Create<GrpcUser.GrpcUserClient>(httpClient);

            try
            {
                var response = await client.GetUserAsync(new GetUserRequest { Id = "123" });
                Console.WriteLine("Response from server : ");
                Console.WriteLine("Id: " + response.Id);
                Console.WriteLine("Name: " + response.Name);
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.NotFound)
                {
                    Console.WriteLine(ex.Status.Detail);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
