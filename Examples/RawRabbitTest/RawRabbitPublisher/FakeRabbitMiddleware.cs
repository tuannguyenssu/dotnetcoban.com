using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace RawRabbitPublisher
{
    public class FakeRabbitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEventPublisher _eventPublisher;

        public FakeRabbitMiddleware(RequestDelegate next, IEventPublisher eventPublisher)
        {
            _next = next;
            _eventPublisher = eventPublisher;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var product = new ProductCreated()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString()
            };
            await _eventPublisher.PublishMessage<ProductCreated>(product);
            await _next(httpContext);
        }
    }
}
