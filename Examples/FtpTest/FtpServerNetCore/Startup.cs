using System;
using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FtpServerNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFtpServer(options =>
            {
                //options.UseDotNetFileSystem().EnableAnonymousAuthentication();
                options.UseDotNetFileSystem();
            }).AddSingleton<IMembershipProvider, CustomMembershipProvider>();
            services.Configure<FtpServerOptions>(options =>
            {
                options.ServerAddress = "*";
                options.Port = 21;
            });

            services.Configure<DotNetFileSystemOptions>(options =>
            {
                options.RootPath = $"{AppContext.BaseDirectory}/FtpRootFolder";
                options.AllowNonEmptyDirectoryDelete = true;
            });
            services.AddHostedService<HostedFtpService>();
        }

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
                    await context.Response.WriteAsync("FTP Server hosted on Asp .NET Core");
                });
            });
        }
    }
}
