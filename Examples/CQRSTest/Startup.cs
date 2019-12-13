using CQRSTest.Filters;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace CQRSTest
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
            // Cấu hình MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            // Cấu hình Exception filter trong đó có bao gồm FluentValidation
            //services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            // Cấu hình FluentValidation
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
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
                    await context.Response.WriteAsync("AspNetCore CQRS Test");
                });
                endpoints.MapControllers();
            });
        }
    }
}
