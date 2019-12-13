using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using System;

namespace CAPEventBusTest
{
    public interface ISubscriberService
    {
        void CheckReceivedMessage(DateTime datetime);
    }

    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        private readonly ILogger<SubscriberService> _logger;

        public SubscriberService(ILogger<SubscriberService> logger)
        {
            _logger = logger;
        }

        [CapSubscribe("test.show.time")]
        public void CheckReceivedMessage(DateTime datetime)
        {
            _logger.LogInformation("message time is:" + datetime);
        }
    }
}
