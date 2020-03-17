using AspNetCoreMicroservicesTest.IntegrationHandler;
using AspNetCoreMicroservicesTest.Messaging.CorrelationId;
using AspNetCoreMicroservicesTest.Messaging.Tracing;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

// Ref from https://github.com/yesmarket/MassTransit.OpenTracing
namespace AspNetCoreMicroservicesTest.Messaging
{

    public static class ServiceCollectionExtension
    {
        private const string RabbitMqSection = "rabbitMQ";
        public static IServiceCollection AddCustomMassTransit(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using var scope = resolver.CreateScope();
            var config = scope.ServiceProvider.GetService<IConfiguration>();

            var rabbitMqOptions = new RabbitMqOptions();
            config.Bind(RabbitMqSection, rabbitMqOptions);

            services.AddSingleton(rabbitMqOptions);

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<Startup>();
                cfg.AddRabbitMqBus((provider, rabbitConfig) =>
                {
                    rabbitConfig.Host(new Uri(rabbitMqOptions.Url), "/", hostConfig =>
                    {
                        hostConfig.Username(rabbitMqOptions.UserName);
                        hostConfig.Password(rabbitMqOptions.Password);
                    });

                    rabbitConfig.ReceiveEndpoint("event", x =>
                    {
                        x.ConfigureConsumer<EventCreatedConsumer>(provider);
                    });

                    rabbitConfig.PropagateOpenTracingContext();
                    rabbitConfig.PropagateCorrelationIdContext();
                });

            });
            return services;
        }

        public static void PropagateOpenTracingContext(this IBusFactoryConfigurator value)
        {
            value.ConfigurePublish(c => c.AddPipeSpecification(new OpenTracingPipeSpecification()));
            value.AddPipeSpecification(new OpenTracingPipeSpecification());
        }

        public static void PropagateCorrelationIdContext(this IBusFactoryConfigurator value)
        {
            value.AddPipeSpecification(new CorrelationLoggerSpecification());
        }

        public static string GetExchangeName(this Uri value)
        {
            var exchange = value.LocalPath;
            var messageType = exchange.Substring(exchange.LastIndexOf('/') + 1);
            return messageType;
        }
    }
}
