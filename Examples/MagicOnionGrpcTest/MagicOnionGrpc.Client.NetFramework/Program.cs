using System;
using System.Threading.Tasks;
using System.Timers;
using Grpc.Core;
using MagicOnion.Client;
using MagicOnionGrpc.Contract;

namespace MagicOnionGrpc.Client
{
    class Program
    {
        private static ISampleStreamingHub _sampleStreamingHub;
        static async Task Main(string[] args)
        {
            var options = new[]
            {
                // send keepalive ping every 10 second, default is 2 hours
                new ChannelOption("grpc.keepalive_time_ms", 10000),
                // keepalive ping time out after 5 seconds, default is 20 seoncds
                new ChannelOption("grpc.keepalive_timeout_ms", 5000),
                // allow grpc pings from client every 10 seconds
                new ChannelOption("grpc.http2.min_time_between_pings_ms", 10000),
                // allow unlimited amount of keepalive pings without data
                new ChannelOption("grpc.http2.max_pings_without_data", 0),
                // allow keepalive pings when there's no gRPC calls
                new ChannelOption("grpc.keepalive_permit_without_calls", 1),
                // allow grpc pings from client without data every 5 seconds
                new ChannelOption("grpc.http2.min_ping_interval_without_data_ms", 5000),
            };

            var channel = new Channel("localhost", 5001, ChannelCredentials.Insecure, options);
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
