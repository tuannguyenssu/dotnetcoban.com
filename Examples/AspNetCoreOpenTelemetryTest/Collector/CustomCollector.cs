using OpenTelemetry.Collector;
using OpenTelemetry.Trace;
using System;

namespace AspNetCoreOpenTelemetryTest.Collector
{
    public class CustomCollector : IDisposable
    {
        private readonly DiagnosticSourceSubscriber _diagnosticSourceSubscriber;

        public CustomCollector(Tracer tracer)
        {
            _diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(new CustomHttpListener("Microsoft.AspNetCore", tracer), null);
            _diagnosticSourceSubscriber.Subscribe();
        }

        public void Dispose()
        {
            _diagnosticSourceSubscriber?.Dispose();
        }
    }
}
