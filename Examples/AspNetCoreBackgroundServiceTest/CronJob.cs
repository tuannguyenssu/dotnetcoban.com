using Cronos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreBackgroundServiceTest
{
    public class JobBase : IDisposable
    {
        private System.Timers.Timer _timer;
        private CronExpression _expression;

        public void SetSchedule(string cronExpression)
        {
            _expression = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
        }

        public virtual async Task RunAsync(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await RunAsync(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await RunAsync(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }

    public class Scheduler
    {
        private readonly List<JobBase> _jobs = new List<JobBase>();

        public void SetJobSchedule(JobBase job, string cronExpression)
        {
            job.SetSchedule(cronExpression);
            _jobs.Add(job);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var job in _jobs)
            {
                tasks.Add(job.RunAsync(cancellationToken));
            }

            await Task.WhenAll(tasks);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var job in _jobs)
            {
                await job.StopAsync(cancellationToken);
                job?.Dispose();
            }
        }
    }

    public static class Extension
    {
        public static IServiceCollection AddCronJob(this IServiceCollection services)
        {
            var jobs = typeof(Startup).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(JobBase)) && !t.IsAbstract);
            foreach (var job in jobs)
            {
                services.AddSingleton(job);
            }
            services.AddSingleton<Scheduler>();
            return services;
        }
    }
}
