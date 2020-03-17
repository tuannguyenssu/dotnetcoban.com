using System;
using AspNetCoreMicroservicesTest.Messaging.CorrelationId;
using AspNetCoreMicroservicesTest.Messaging.Tracing;
using MassTransit;

// Ref from https://github.com/yesmarket/MassTransit.OpenTracing
namespace AspNetCoreMicroservicesTest.Messaging
{
    public static class Extensions
    {
        public static void PropagateOpenTracingContext(this IBusFactoryConfigurator value)
        {
            value.ConfigurePublish(c => c.AddPipeSpecification(new OpenTracingPipeSpecification()));
            value.AddPipeSpecification(new OpenTracingPipeSpecification());
        }

        public static void PropagateCorrelationIdContext(this IBusFactoryConfigurator value)
        {
            // value.Confi(c => c.AddPipeSpecification(new CorrelationLoggerSpecification()));
            value.AddPipeSpecification(new CorrelationLoggerSpecification());
        }

        public static string GetExchangeName(this Uri value)
        {
            var exchange = value.LocalPath;
            var messageType = exchange.Substring(exchange.LastIndexOf('/') + 1);
            return messageType;
        }
    }
}
