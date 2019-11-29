using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AspNetCoreMiddlewareTest.Middleware
{
    public class SimpleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SimpleMiddleware> _logger;

        public SimpleMiddleware(RequestDelegate next, ILogger<SimpleMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments(new PathString("/test")))
            {
                await httpContext.Response.WriteAsync("Test");
            }
            else
            {
                _logger.LogInformation("Simple Middleware is called");

                await _next(httpContext);
            }
        }
    }
}
