using System;
using Quartz;
using System.Threading.Tasks;

namespace QuartzNetTest
{
    public class TestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(DateTime.UtcNow);
            await Task.CompletedTask;
        }
    }
}
