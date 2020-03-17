using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace AspNetCoreCorrelationIdTest
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdOptions _options;

        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Invoke(HttpContext context, ICorrelationContextFactory correlationContextFactory)
        {
            var correlationId = SetCorrelationId(context);

            if (_options.UpdateTraceIdentifier)
            {
                context.TraceIdentifier = correlationId.ToString();
            }

            correlationContextFactory.Create(correlationId, _options.Header);

            if (_options.IncludeInResponse)
            {
                // apply the correlation ID to the response header for client side tracking
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(_options.Header))
                    {
                        context.Response.Headers.Add(_options.Header, correlationId.ToString());
                    }

                    return Task.CompletedTask;
                });
            }

            //using (LogContext.PushProperty(_options.Header, correlationId))
            //{
            //    await _next(context);
            //}

            await _next(context);

            correlationContextFactory.Dispose();
        }

        private Guid SetCorrelationId(HttpContext context)
        {
            var correlationIdFoundInRequestHeader = context.Request.Headers.TryGetValue(_options.Header, out var correlationId);

            if (RequiresGenerationOfCorrelationId(correlationIdFoundInRequestHeader, correlationId))
            {
                return Guid.NewGuid();
            }
            else
            {
                return Guid.Parse(correlationId);
            }
        }

        private static bool RequiresGenerationOfCorrelationId(bool idInHeader, String idFromHeader)
        {
            if (!idInHeader || StringValues.IsNullOrEmpty(idFromHeader) || !Guid.TryParse(idFromHeader, out var correlationId))
            {
                return true;
            }
            return false;
        }
    }
}
