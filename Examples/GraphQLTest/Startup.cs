using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using GraphQLTest.GraphQL;
using GraphQLTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace GraphQLTest
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
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // Inject các interfaces
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();

            // Cấu hình GraphQL
            services.AddScoped<GraphTestSchema>();

            services.AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                    options.ExposeExceptions = true;
                    options.UnhandledExceptionDelegate = ctx =>
                    {
                        Console.WriteLine("error: " + ctx.OriginalException.Message);
                    };
                })
                .AddDataLoader()
                .AddGraphTypes(typeof(GraphTestSchema), ServiceLifetime.Scoped);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Cấu hình GraphQL
            app.UseGraphQL<GraphTestSchema>("/graphql");

            // Cấu hình giao diện Playground
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground",
                PlaygroundSettings = new Dictionary<string, object>
                {
                    ["editor.theme"] = "light",
                    ["tracing.hideTracingResponse"] = false,
                }
            });

            // Cấu hình giao diện Graphiql
            app.UseGraphiQLServer(new GraphiQLOptions
            {
                Path = "/ui/graphiql",
                GraphQLEndPoint = "/graphql",
            });

            // Cấu hình giao diện Voyager
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions
            {
                Path = "/ui/voyager",
                GraphQLEndPoint = "/graphql",
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("AspNetCore GraphQL Test");
                });
                endpoints.MapControllers();
            });
        }
    }
}
