using EventBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EventBusTest
{
    public class TestValueEventHandler : IIntegrationEventHandler<TestValueEvent>
    {
        private readonly ILogger<TestValueEventHandler> _logger;
        public TestValueEventHandler(ILogger<TestValueEventHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(TestValueEvent @event)
        {
            _logger.LogInformation($"----- Received integration event with id : {@event.Id}");

            return Task.CompletedTask;
        }
    }
}
