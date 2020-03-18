using System.Reflection;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace AspNetCoreMicroservicesTest.Tracing.Jaeger
{
    public static class ServiceCollectionExtensions
    {
        private static bool _initialized;

        public static IServiceCollection AddJaeger(this IServiceCollection services)
        {
            if (_initialized)
            {
                return services;
            }

            _initialized = true;

            var options = new JaegerOptions();
            options.Sampler = "const";
            options.Enabled = true;
            options.UdpHost = "localhost";
            options.UdpPort = 6831;
            options.ServiceName = "Test Jaeger";
            options.MaxPacketSize = 0;
            if (!options.Enabled)
            {
                var defaultTracer = new Tracer.Builder(Assembly.GetEntryAssembly().FullName)
                    .WithReporter(new NoopReporter())
                    .WithSampler(new ConstSampler(false))
                    .Build();

                services.AddSingleton<ITracer>(defaultTracer);

                return services;
            }

            services.AddSingleton<ITracer>(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                var reporter = new RemoteReporter.Builder()
                    .WithSender(new UdpSender(options.UdpHost, options.UdpPort, options.MaxPacketSize))
                    .WithLoggerFactory(loggerFactory)
                    .Build();

                var sampler = GetSampler(options);

                var tracer = new Tracer.Builder(options.ServiceName)
                    .WithReporter(reporter)
                    .WithSampler(sampler)
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();

            return services;
        }

        public static IApplicationBuilder UseJaeger(this IApplicationBuilder app)
        {
            app.UseMiddleware<JaegerHttpMiddleware>();

            return app;
        }

        private static ISampler GetSampler(JaegerOptions options)
        {
            switch (options.Sampler)
            {
                case "rate":
                    return new RateLimitingSampler(options.MaxTracesPerSecond);
                case "probabilistic":
                    return new ProbabilisticSampler(options.SamplingRate);
                default:
                    return  new ConstSampler(true);
            }
        }
    }
}
