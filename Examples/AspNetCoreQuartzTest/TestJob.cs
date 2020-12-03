using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace AspNetCoreQuartzTest
{
    [DisallowConcurrentExecution]
    public class TestJob : IJob
    {
        private readonly ILogger<TestJob> _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{nameof(TestJob)} is running at {DateTime.Now}");
            await Task.CompletedTask;
        }
    }
}
