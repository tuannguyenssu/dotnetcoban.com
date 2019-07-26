using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savorboard.CAP.InMemoryMessageQueue;

namespace CAPEventBusTest
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
            //services.AddDbContext<AppDbContext>();

            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddCap(x =>
            {
                x.UseDashboard();
                x.UseInMemoryStorage();
                x.UseInMemoryMessageQueue();

                // If you are using EF, you need to add the configuration：
                //x.UseEntityFramework<AppDbContext>(); //Options, Notice: You don't need to config x.UseSqlServer(""") again! CAP can autodiscovery.

                //// If you are using ADO.NET, choose to add configuration you needed：
                //string sqlConnectionString = "Server=localhost,1433;Database=CAPTestDb;User=sa;Password=dbgpftm;";
                //x.UseSqlServer(sqlConnectionString);

                //x.UseRabbitMQ(o =>
                //{
                //    o.HostName = "localhost";
                //    o.ConnectionFactoryOptions = opt => {
                //        //rabbitmq client ConnectionFactory config
                //        opt.UserName = "guest";
                //        opt.Password = "guest";
                //    };
                //});
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
