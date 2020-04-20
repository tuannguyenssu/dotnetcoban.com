using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProxyKit;

namespace AspNetCoreProxyKitTest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProxy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("", api =>
            {
                api.RunProxy(async context =>
                {
                    var forwardContext = context
                        .ForwardTo("https://dotnetcoban.com")
                        .CopyXForwardedHeaders();

                    return await forwardContext.Send();
                });
            });

            app.Map("/google", api =>
            {
                api.RunProxy(async context =>
                {
                    var forwardContext = context
                        .ForwardTo("https://google.com")
                        .CopyXForwardedHeaders();

                    return await forwardContext.Send();
                });
            });
        }
    }
}
