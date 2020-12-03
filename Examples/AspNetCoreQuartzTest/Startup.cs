using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace AspNetCoreQuartzTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                q.AddJob<TestJob>(options =>
                {
                    options.WithIdentity($"{nameof(TestJob)}");
                });
                q.AddTrigger(options =>
                {
                    options.ForJob($"{nameof(TestJob)}").WithIdentity($"{nameof(TestJob)}")
                        .WithCronSchedule("0/2 * * * * ?");
                });
            });

            services.AddQuartzHostedService(q =>
            {
                q.WaitForJobsToComplete = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Asp Net Core Quartz Test");
                });
            });
        }
    }
}
