using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMicroservicesTest.IntegrationEvent;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMicroservicesTest.IntegrationHandler
{
    public struct ConsumerAnchor
    {

    }

    public class EventCreatedConsumer : IConsumer<EventCreated>
    {
        private readonly ILogger<EventCreatedConsumer> _logger;

        public EventCreatedConsumer(ILogger<EventCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<EventCreated> context)
        {
            _logger.LogInformation("EventCreatedConsumer - happened with correlation Id {0}", context.Message.CorrelationId);
            return Task.CompletedTask;
        }
    }
}
