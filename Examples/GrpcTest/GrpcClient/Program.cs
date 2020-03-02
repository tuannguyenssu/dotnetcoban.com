using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client.Web;
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

            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });

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
                Console.WriteLine(ex.Status.Detail);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
