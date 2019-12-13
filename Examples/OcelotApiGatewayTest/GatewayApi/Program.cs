using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace GatewayApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("hostsettings.json", true, true)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .AddJsonFile("ocelot.json", true, true)
                    .AddJsonFile($"ocelot.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .AddEnvironmentVariables(prefix: "PREFIX_")
                    .AddCommandLine(args).Build();

                CreateHostBuilder(args, config).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(config);
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
