using System;
using MagicOnion;
using MessagePack;

namespace MagicOnionGrpc.Contract
{
    public interface IGrpcService : IService<IGrpcService>
    {
        UnaryResult<PingResponse> PingAsync(PingRequest request);

        UnaryResult<int> SumAsync(int x, int y);
    }

    [MessagePackObject]
    public class PingRequest
    {
        [Key(0)]
        public string Ping { get; set; }
    }

    [MessagePackObject]
    public class PingResponse
    {
        [Key(0)]
        public string Pong { get; set; }
    }
}
