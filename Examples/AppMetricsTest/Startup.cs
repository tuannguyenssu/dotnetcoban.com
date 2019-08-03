using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppMetricsTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var database = "appmetricsdemo";
            var uri = new Uri("http://127.0.0.1:8086");

            services.AddMetrics(options =>
            {
                options.WithGlobalTags((globalTags, info) =>
                {
                    globalTags.Add("app", info.EntryAssemblyName);
                    globalTags.Add("env", "stage");
                });
            })
                .AddHealthChecks()
                .AddReporting(
                    factory =>
                    {
                        factory.AddInfluxDb(
                            new InfluxDBReporterSettings
                            {
                                InfluxDbSettings = new InfluxDBSettings(database, uri),
                                ReportInterval = TimeSpan.FromSeconds(5)
                            });
                    })
                .AddMetricsMiddleware(options => options.IgnoredHttpStatusCodes = new[] { 404 });

            services.AddMvc(options => options.AddMetricsResourceFilter()).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMetrics();
            app.UseMetricsReporting(lifetime);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
