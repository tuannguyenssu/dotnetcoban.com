using System.Reflection;
using CQRSTest.Application.Behaviors;
using CQRSTest.DataAccess;
using CQRSTest.Domain.Customers;
using CQRSTest.Filters;
using CQRSTest.Middlewares;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            //services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            // Cấu hình Exception filter trong đó có bao gồm FluentValidation
            //services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            services.AddControllers()
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

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

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
