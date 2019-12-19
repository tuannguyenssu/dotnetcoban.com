using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Refit;

namespace AspNetCoreHttpClientTest
{
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Basic usage
            services.AddHttpClient();

            // Named clients
            services.AddHttpClient("github", c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");
                // Github API versioning
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                // Github requires a user-agent
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });

            services.AddHttpClient<GithubService>(c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            })
            .AddTransientHttpErrorPolicy(p => p.RetryAsync(3))
            .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            // Refit
            services.AddHttpClient("RefitGithub", c =>
                {
                    c.BaseAddress = new Uri("https://api.github.com");
                })
                .AddTypedClient(c => RestService.For<IGitHubApi>(c));

            //services.AddRefitClient<IGitHubApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.github.com"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>
            {
                var service = context.RequestServices.GetRequiredService<IGitHubApi>();
                var response = await service.GetUser("tuannguyenssu");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });

            //app.Run(async context =>
            //{
            //    var service = context.RequestServices.GetRequiredService<GithubService>();

            //    var response = await service.GetUserInfoAAsync();

            //    await context.Response.WriteAsync(response);
            //});

            //app.Run(async context =>
            //{
            //    var clientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();

            //    var request = new HttpRequestMessage(HttpMethod.Get,
            //        "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");

            //    var client = clientFactory.CreateClient("github");

            //    var response = await client.SendAsync(request);

            //    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            //});


            //app.Run(async context =>
            //{
            //    var clientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();

            //    var request = new HttpRequestMessage(HttpMethod.Get,
            //        "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
            //    request.Headers.Add("Accept", "application/vnd.github.v3+json");
            //    request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            //    var client = clientFactory.CreateClient();

            //    var response = await client.SendAsync(request);

            //    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            //});
        }
    }
}
