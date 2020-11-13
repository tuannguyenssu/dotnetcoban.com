using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreBackgroundServiceTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var scheduler = host.Services.GetRequiredService<Scheduler>();

            var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            applicationLifetime.ApplicationStopped.Register(async () =>
            {
                await scheduler.StopAsync(CancellationToken.None);
            });

            var serviceA = host.Services.GetRequiredService<ProducerJob>();
            scheduler.SetJobSchedule(serviceA, "* * * * * *");

            var serviceB = host.Services.GetRequiredService<ConsumerJob>();
            scheduler.SetJobSchedule(serviceB, "*/5 * * * * *");

            await scheduler.StartAsync(CancellationToken.None);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
