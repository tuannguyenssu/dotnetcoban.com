using AspNetCoreMicroservicesTest.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AspNetCoreMicroservicesTest.Logging
{
    public static class Extensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, string applicationName = "")
        {
            hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var appOptions = new AppOptions();
                context.Configuration.GetSection("App").Bind(appOptions);
                var loggingOptions = new LoggingOptions();
                context.Configuration.GetSection("Logging").Bind(loggingOptions);

                applicationName = string.IsNullOrWhiteSpace(applicationName) ? appOptions.Name : applicationName;

                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration, "Logging")
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", applicationName);

                if (loggingOptions.ConsoleEnabled)
                {
                    loggerConfiguration.WriteTo
                        .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {Properties:j}] {Message:lj}{NewLine}{Exception}");
                }
                if (loggingOptions.Seq.Enabled)
                {
                    loggerConfiguration.WriteTo.Seq(loggingOptions.Seq.Url, apiKey: loggingOptions.Seq.ApiKey);
                }
            });

            return hostBuilder;
        }
    }
}
