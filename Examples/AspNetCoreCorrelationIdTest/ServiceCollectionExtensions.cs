using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AspNetCoreCorrelationIdTest
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelationId(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
            serviceCollection.TryAddTransient<ICorrelationContextFactory, CorrelationContextFactory>();

            return serviceCollection;
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            app.NotNull();
            return app.UseCorrelationId(new CorrelationIdOptions());
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, string header)
        {
            app.NotNull();
            return app.UseCorrelationId(new CorrelationIdOptions
            {
                Header = header
            });
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, CorrelationIdOptions options)
        {
            app.NotNull();
            options.NotNull();
            if (app.ApplicationServices.GetService(typeof(ICorrelationContextFactory)) == null)
            {
                throw new InvalidOperationException("Unable to find the required services. You must call the AddCorrelationId method in ConfigureServices in the application startup code.");
            }

            return app.UseMiddleware<CorrelationIdMiddleware>(Options.Create(options));
        }
    }
}
