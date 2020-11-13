using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreBackgroundServiceTest
{
    public class ProducerJob : JobBase
    {
        private readonly MessageQueueProcessor _messageQueue;
        private readonly ILogger<ProducerJob> _logger;

        public ProducerJob(ILogger<ProducerJob> logger, MessageQueueProcessor messageQueue)
        {
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public override Task RunAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation($"{nameof(ProducerJob)} started!");
            return base.RunAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ProducerJob)} stopped!");
            return base.StopAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            //_logger.LogInformation($"{nameof(ProducerJob)} running at: {DateTimeOffset.Now}");
            await _messageQueue.AddDataAsync(1);
        }
    }
}
