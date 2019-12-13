using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using GrpcUserEndpoint;
using static GrpcUserEndpoint.UserService;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting GRPC Client...");
            Console.WriteLine();

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UserServiceClient(channel);
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
