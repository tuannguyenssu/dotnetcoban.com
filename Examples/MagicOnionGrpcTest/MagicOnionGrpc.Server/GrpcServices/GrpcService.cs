using System;
using MagicOnion;
using MagicOnion.Server;
using MagicOnionGrpc.Contract;

namespace MagicOnionGrpc.Server.GrpcServices
{
    public class GrpcService : ServiceBase<IGrpcService>, IGrpcService
    {
        public UnaryResult<PingResponse> PingAsync(PingRequest request)
        {
            return new UnaryResult<PingResponse>(new PingResponse()
            {
                Pong = request.Ping
            });
        }

        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Console.WriteLine($"Received:{x}, {y}");
            return x + y;
        }
    }
}
