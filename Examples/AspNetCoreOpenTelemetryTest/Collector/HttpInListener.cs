using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using OpenTelemetry.Collector;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AspNetCoreOpenTelemetryTest.Collector
{
    internal class CustomHttpListener : ListenerHandler
    {
        private static readonly string UnknownHostName = "UNKNOWN-HOST";
        private readonly PropertyFetcher startContextFetcher = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher stopContextFetcher = new PropertyFetcher("HttpContext");

        public CustomHttpListener(string name, Tracer tracer)
            : base(name, tracer)
        {
        }

        public override void OnStartActivity(Activity activity, object payload)
        {
            var operationName = activity.OperationName;
            const string EventNameSuffix = ".OnStartActivity";
            var context = this.startContextFetcher.Fetch(payload) as HttpContext;

            if (context == null)
            {
                CollectorEventSource.Log.NullPayload(nameof(CustomHttpListener) + EventNameSuffix);
                return;
            }

            if (context.Request.Path != "/users")
            {
                CollectorEventSource.Log.RequestIsFilteredOut(activity.OperationName);
                return;
            }


            //if (this.options.RequestFilter != null && !this.options.RequestFilter(context))
            //{
            //    CollectorEventSource.Log.RequestIsFilteredOut(activity.OperationName);
            //    return;
            //}

            var request = context.Request;

            // see the spec https://github.com/open-telemetry/opentelemetry-specification/blob/master/specification/data-semantic-conventions.md
            var path = (request.PathBase.HasValue || request.Path.HasValue) ? (request.PathBase + request.Path).ToString() : "/";

            Tracer.StartActiveSpanFromActivity(path, Activity.Current, SpanKind.Server, out var span);

            //var ctx = new TraceContextFormat().Extract<HttpRequest>(request, (r, name) => r.Headers[name]);
            //Tracer.StartActiveSpan(path, ctx, SpanKind.Server, out span);

            if (span.IsRecording)
            {
                // Note, route is missing at this stage. It will be available later
                span.PutHttpHostAttribute(request.Host.Host, request.Host.Port ?? 80);
                span.PutHttpMethodAttribute(request.Method);
                span.PutHttpPathAttribute(path);

                var userAgent = request.Headers["User-Agent"].FirstOrDefault();
                span.PutHttpUserAgentAttribute(userAgent);
                span.PutHttpRawUrlAttribute(GetUri(request));
            }
        }

        public override void OnStopActivity(Activity activity, object payload)
        {
            const string EventNameSuffix = ".OnStopActivity";
            var span = this.Tracer.CurrentSpan;

            if (span == null || !span.Context.IsValid)
            {
                CollectorEventSource.Log.NullOrBlankSpan(nameof(CustomHttpListener) + EventNameSuffix);
                return;
            }

            if (span.IsRecording)
            {
                if (!(this.stopContextFetcher.Fetch(payload) is HttpContext context))
                {
                    CollectorEventSource.Log.NullPayload(nameof(CustomHttpListener) + EventNameSuffix);
                    return;
                }

                var response = context.Response;

                span.PutHttpStatusCode(response.StatusCode, response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase);
            }

            span.End();
        }

        private static string GetUri(HttpRequest request)
        {
            var builder = new StringBuilder();

            builder.Append(request.Scheme).Append("://");

            if (request.Host.HasValue)
            {
                builder.Append(request.Host.Value);
            }
            else
            {
                // HTTP 1.0 request with NO host header would result in empty Host.
                // Use placeholder to avoid incorrect URL like "http:///"
                builder.Append(UnknownHostName);
            }

            if (request.PathBase.HasValue)
            {
                builder.Append(request.PathBase.Value);
            }

            if (request.Path.HasValue)
            {
                builder.Append(request.Path.Value);
            }

            if (request.QueryString.HasValue)
            {
                builder.Append(request.QueryString);
            }

            return builder.ToString();
        }
    }
}