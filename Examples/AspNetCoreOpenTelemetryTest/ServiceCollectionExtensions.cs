using AspNetCoreOpenTelemetryTest.Collector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using OpenTelemetry.Trace.Samplers;

namespace AspNetCoreOpenTelemetryTest
{
    public static class ServiceCollectionExtensions
    {
        private static bool _initialized;

        private const string OpenTelemetrySectionName = "OpenTelemetry";
        public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
        {
            if (_initialized)
            {
                return services;
            }

            _initialized = true;

            var resolver = services.BuildServiceProvider();
            using var scope = resolver.CreateScope();
            var config = scope.ServiceProvider.GetService<IConfiguration>();

            var openTelemetryOptions = new OpenTelemetryOptions();
            config.Bind(OpenTelemetrySectionName, openTelemetryOptions);
            services.AddSingleton(openTelemetryOptions);
            services.AddOpenTelemetry((serviceProvider, builder) =>
            {
                builder.SetSampler(GetSampler(openTelemetryOptions));

                builder.UseZipkin(o =>
                {
                    o.ServiceName = openTelemetryOptions.ServiceName;
                    o.Endpoint = new System.Uri(openTelemetryOptions.ZipkinEndpoint);
                });
                //builder.AddRequestCollector();
                //builder.AddDependencyCollector();

                builder.AddCollector(t => new CustomCollector(t));
            });

            return services;
        }

        private static Sampler GetSampler(OpenTelemetryOptions options)
        {
            return options.Sampler switch
            {
                "const" => new AlwaysSampleSampler(),
                _ => new AlwaysSampleSampler(),
            };
        }
    }
}
