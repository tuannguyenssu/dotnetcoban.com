using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreBackgroundServiceTest
{
    public class CronServiceA : CronJobService
    {
        private readonly ILogger<CronServiceA> _logger;

        public CronServiceA(ScheduleConfig<CronServiceA> config, ILogger<CronServiceA> logger) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CronServiceA)} started!");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CronServiceA)} stopped!");
            return base.StopAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CronServiceA)} running at: {DateTimeOffset.Now}");
            return Task.CompletedTask;
        }
    }
}
