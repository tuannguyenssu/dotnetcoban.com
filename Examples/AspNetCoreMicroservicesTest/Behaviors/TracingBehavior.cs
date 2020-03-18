using System.Threading;
using System.Threading.Tasks;
using AspNetCoreMicroservicesTest.Shared;
using MediatR;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;

namespace AspNetCoreMicroservicesTest.Behaviors
{
    public class TracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Tracer _tracer;

        public TracingBehavior(TracerFactory tracerFactory)
        {
            _tracer = tracerFactory.GetTracer("MediatR behavior");
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (_tracer.StartActiveSpan(request.GetGenericTypeName(), out var span)) 
            {
                var response = await next();
                return response;
            }
        }
    }
}