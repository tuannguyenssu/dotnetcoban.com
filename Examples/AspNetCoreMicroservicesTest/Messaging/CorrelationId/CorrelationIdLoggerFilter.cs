using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Serilog.Context;

namespace AspNetCoreMicroservicesTest.Messaging.CorrelationId
{
    public class CorrelationIdLoggerFilter : IFilter<ConsumeContext>
    {
        public void Probe(ProbeContext context) { }

        public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            using (LogContext.PushProperty("X-Correlation-ID", context.CorrelationId.ToString()))
            {
                await next.Send(context);
            }
        }
    }
}
