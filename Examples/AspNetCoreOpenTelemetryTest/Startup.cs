using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using System.Threading;

namespace AspNetCoreOpenTelemetryTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomOpenTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TracerFactory tracerFactory, OpenTelemetryOptions openTelemetryOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/users", async httpContext =>
                {
                    var tracer = tracerFactory.GetTracer("front-end-service-tracer");


                    var mainSpan = tracer.StartSpan("Start the request span", SpanKind.Server);


                    var userTracer = TracerFactory.Create(t => t.UseZipkin(c =>
                    {
                        c.ServiceName = "user-service";
                        c.Endpoint = new System.Uri(openTelemetryOptions.ZipkinEndpoint);
                    })).GetTracer("user-service-tracer");

                    var userSpan = userTracer.StartSpan("Step 1 : Create user request span", mainSpan, SpanKind.Server);


                    var grpcSpan = userTracer.StartSpan("Step 1-1 : Call gRPC service to create user", userSpan, SpanKind.Client);
                    Thread.Sleep(200);
                    grpcSpan.End();

                    var processResponseSpan = userTracer.StartSpan("Step 1-2: Parse user info", userSpan, SpanKind.Server);
                    Thread.Sleep(50);
                    processResponseSpan.End();

                    userSpan.End();

                    var busTracer = TracerFactory.Create(t => t.UseZipkin(c =>
                    {
                        c.ServiceName = "message-bus-service";
                        c.Endpoint = new System.Uri(openTelemetryOptions.ZipkinEndpoint);
                    })).GetTracer("message-bus-service-tracer");

                    var busSpan = busTracer.StartSpan("Step 2 : Process create user event", mainSpan, SpanKind.Server);

                    var publishSpan = busTracer.StartSpan("Step 2-1 : Publish event to bus service", busSpan, SpanKind.Consumer);
                    Thread.Sleep(100);
                    publishSpan.End();

                    var consumeSpan = busTracer.StartSpan("Step 2-2 : Consume event from bus service", busSpan, SpanKind.Consumer);
                    Thread.Sleep(100);
                    consumeSpan.End();

                    busSpan.End();

                    var emailTracer = TracerFactory.Create(t => t.UseZipkin(c =>
                    {
                        c.ServiceName = "email-service";
                        c.Endpoint = new System.Uri(openTelemetryOptions.ZipkinEndpoint);
                    })).GetTracer("email-service-tracer");

                    var emailSpan = emailTracer.StartSpan("Step 3 : Send a confirmation email to user", mainSpan, SpanKind.Server);

                    var sendGridSpan = emailTracer.StartSpan("Step 3-1 : Call SendGrid email service", emailSpan, SpanKind.Client);
                    Thread.Sleep(100);
                    sendGridSpan.End();

                    emailSpan.End();

                    mainSpan.End();

                    var completeSpan = tracer.StartSpan("Finish the request span", SpanKind.Server);

                    completeSpan.End();

                    await httpContext.Response.WriteAsync("Product List");
                });
            });
        }
    }
}
