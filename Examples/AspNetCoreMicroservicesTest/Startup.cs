using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMicroservicesTest.Behaviors;
using AspNetCoreMicroservicesTest.CorrelationId;
using AspNetCoreMicroservicesTest.Messaging;
using AspNetCoreMicroservicesTest.Shared;
using AspNetCoreMicroservicesTest.Tracing.Jaeger;
using AspNetCoreMicroservicesTest.Tracing.OpenTelemetry;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Senders;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace AspNetCoreMicroservicesTest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();
            services.AddCustomOpenTelemetry();
            services.AddJaeger();
            services.AddCustomMassTransit();
            services.AddMediatR(typeof(Startup));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationId();

            app.UseJaeger();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("AspNetCore Microservices Test");
                });

                endpoints.MapControllers();
            });
        }


    }


}
