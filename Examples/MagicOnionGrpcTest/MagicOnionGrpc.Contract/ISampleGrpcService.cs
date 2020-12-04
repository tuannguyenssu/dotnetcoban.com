using MagicOnion;
using MessagePack;

namespace MagicOnionGrpc.Contract
{
    public interface ISampleGrpcService : IService<ISampleGrpcService>
    {
        UnaryResult<PingResponse> PingAsync(PingRequest request);
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
