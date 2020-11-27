using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace MagicOnionGrpc.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.ListenAnyIP(5000);
                        // Http2
                        options.ListenAnyIP(5001, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });

                        //options.ConfigureEndpointDefaults(endpointOptions =>
                        //{
                        //    endpointOptions.Protocols = HttpProtocols.Http2;
                        //});
                    });


                    webBuilder.UseStartup<Startup>();
                });
    }
}
