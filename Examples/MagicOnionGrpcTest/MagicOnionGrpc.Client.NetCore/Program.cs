using System;
using System.Threading.Tasks;
using System.Timers;
using Grpc.Net.Client;
using MagicOnion.Client;
using MagicOnionGrpc.Contract;

namespace MagicOnionGrpc.Client.NetCore
{
    class Program
    {
        private static ISampleStreamingHub _sampleStreamingHub;
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("http://localhost:5001");

            var grpcService = MagicOnionClient.Create<ISampleGrpcService>(channel);
            var request = new PingRequest()
            {
                Ping = Guid.NewGuid().ToString()
            };
            var response = await grpcService.PingAsync(request);
            Console.WriteLine($"Request : {request.Ping}");
            Console.WriteLine($"Response : {response.Pong}");

            _sampleStreamingHub = StreamingHubClient.Connect<ISampleStreamingHub, ISampleStreamingReceiver>(channel, new SampleStreamingHubReceiver());

            var timer = new Timer(200)
            {
                AutoReset = true,
            };
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
            Console.ReadKey();
        }

        private static async void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {

            await _sampleStreamingHub.JoinAsync(new JoinRequest()
            {
                GroupName = "Sport Group",
                UserName = Guid.NewGuid().ToString()
            });
            await _sampleStreamingHub.SendMessageAsync($"{Guid.NewGuid()}");
            await _sampleStreamingHub.LeaveAsync();

        }
    }
}
