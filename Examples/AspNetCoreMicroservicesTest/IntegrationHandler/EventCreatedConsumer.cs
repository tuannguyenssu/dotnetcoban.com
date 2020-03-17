using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMicroservicesTest.IntegrationEvent;
using MassTransit;

namespace AspNetCoreMicroservicesTest.IntegrationHandler
{
    public class EventCreatedConsumer : IConsumer<EventCreated>
    {
        public Task Consume(ConsumeContext<EventCreated> context)
        {
            return Task.CompletedTask;
        }
    }
}
