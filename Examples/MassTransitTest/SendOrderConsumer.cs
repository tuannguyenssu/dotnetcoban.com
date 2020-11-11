using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MassTransitTest
{
    public class SendOrderConsumer : IConsumer<Order>
    {
        private readonly ILogger<SendOrderConsumer> _logger;

        public SendOrderConsumer(ILogger<SendOrderConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation($"Received order data: {context.Message.Data}");
            return Task.CompletedTask;
        }
    }
}