using MagicOnion;
using MagicOnion.Server;
using MagicOnionGrpc.Contract;
using MagicOnionGrpc.Server.Filters;

namespace MagicOnionGrpc.Server.GrpcServices
{
    public class SampleGrpcService : ServiceBase<ISampleGrpcService>, ISampleGrpcService
    {
        [FromTypeFilter(typeof(LoggingFilterAttribute))]
        public UnaryResult<PingResponse> PingAsync(PingRequest request)
        {
            return new UnaryResult<PingResponse>(new PingResponse()
            {
                Pong = request.Ping
            });
        }
    }
}
