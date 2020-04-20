using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzNetTest
{
    public class JobScheduler
    {
        public static async Task Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            var job = JobBuilder.Create<TestJob>().Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInSeconds(2).RepeatForever()).Build();
            await scheduler.ScheduleJob(job, trigger);
        }


        //https://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/crontriggers.html
        //public static async Task Start()
        //{
        //    var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //    await scheduler.Start();
        //    var job = JobBuilder.Create<TestJob>().Build();
        //    var trigger = TriggerBuilder.Create().WithCronSchedule("0/2 * * * * ?").Build();
        //    await scheduler.ScheduleJob(job, trigger);
        //}
    }
}
