using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMicroservicesTest.CorrelationId;
using AspNetCoreMicroservicesTest.Messaging;
using AspNetCoreMicroservicesTest.Shared;
using AspNetCoreMicroservicesTest.Tracing.OpenTelemetry;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMicroservicesTest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCorrelationId()
                .AddCustomOpenTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }


    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit();
            //services.AddMassTransit((provider) =>
            //{
            //    var rabbitMqOption = configuration.GetOptions<RabbitMqOptions>("rabbitMQ");

            //    return Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        var host = cfg.Host(new Uri(rabbitMqOption.Url), "/", hc =>
            //        {
            //            hc.Username(rabbitMqOption.UserName);
            //            hc.Password(rabbitMqOption.Password);
            //        });

            //        cfg.ReceiveEndpoint("contact", x =>
            //        {
            //            x.ConfigureConsumer<ContactCreatedConsumer>(provider);
            //        });

            //        cfg.PropagateOpenTracingContext();
            //        cfg.PropagateCorrelationIdContext();
            //    });
            //}, (cfg) =>
            //{
            //    cfg.AddConsumersFromNamespaceContaining<ConsumerAnchor>();
            //});

            return services;
        }
    }
}
