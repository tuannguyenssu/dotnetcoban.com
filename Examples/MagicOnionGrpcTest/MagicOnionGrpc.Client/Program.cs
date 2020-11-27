using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using MagicOnionGrpc.Contract;

namespace MagicOnionGrpc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            //var channel = new Grpc.Core.Channel("localhost", 5001, ChannelCredentials.Insecure);
            //var channel = new Channel("localhost", 5001, ChannelCredentials.Insecure);

            //var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions()
            //{
            //    Credentials = ChannelCredentials.Insecure,
            //});

            var channel = GrpcChannel.ForAddress("http://localhost:5001");

            var grpcService = MagicOnionClient.Create<IGrpcService>(channel);
            var request = new PingRequest()
            {
                Ping = Guid.NewGuid().ToString()
            };
            //var response = await grpcService.PingAsync(request);
            //Console.WriteLine($"Request : {request.Ping}");
            //Console.WriteLine($"Response : {response.Pong}");
            var response = await grpcService.SumAsync(10, 20);
            Console.WriteLine(response);
            Console.ReadKey();
        }
    }
}
