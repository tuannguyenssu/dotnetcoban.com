using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCoreBackgroundServiceTest
{
    public class ConsumerJob : JobBase
    {
        private readonly MessageQueueProcessor _messageQueue;
        private readonly ILogger<ConsumerJob> _logger;

        public ConsumerJob(ILogger<ConsumerJob> logger, MessageQueueProcessor messageQueue)
        {
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public override Task RunAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation($"{nameof(ConsumerJob)} started!");
            return base.RunAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ConsumerJob)} stopped!");
            return base.StopAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            //_logger.LogInformation($"{nameof(ConsumerJob)} running at: {DateTimeOffset.Now}");

            await _messageQueue.ProcessDataAsync();
        }
    }
}