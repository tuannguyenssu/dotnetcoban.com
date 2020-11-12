using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCoreBackgroundServiceTest
{
    public class CronServiceB : CronJobService
    {
        private readonly ILogger<CronServiceB> _logger;

        public CronServiceB(ScheduleConfig<CronServiceB> config, ILogger<CronServiceB> logger) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CronServiceB)} started!");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CronServiceB)} stopped!");
            return base.StopAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CronServiceB)} running at: {DateTimeOffset.Now}");
            return Task.CompletedTask;
        }
    }
}