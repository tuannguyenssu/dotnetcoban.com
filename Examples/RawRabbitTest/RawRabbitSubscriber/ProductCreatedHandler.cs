using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RawRabbitSubscriber
{
    public class ProductCreatedHandler : INotificationHandler<ProductCreated>
    {
        private ILogger<ProductCreatedHandler> _logger;

        public ProductCreatedHandler(ILogger<ProductCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductCreated notification, CancellationToken cancellationToken)
        {
            var @event = JsonSerializer.Serialize(notification);
            _logger.LogInformation(@event);
            return Task.CompletedTask;
        }
    }
}
