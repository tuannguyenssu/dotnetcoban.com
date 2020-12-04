using MagicOnion;
using MagicOnion.Server;
using MagicOnionGrpc.Contract;

namespace MagicOnionGrpc.Server.GrpcServices
{
    public class SampleGrpcService : ServiceBase<ISampleGrpcService>, ISampleGrpcService
    {
        public UnaryResult<PingResponse> PingAsync(PingRequest request)
        {
            return new UnaryResult<PingResponse>(new PingResponse()
            {
                Pong = request.Ping
            });
        }
    }
}
