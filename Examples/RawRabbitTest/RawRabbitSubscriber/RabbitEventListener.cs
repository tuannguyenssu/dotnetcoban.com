using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using System;
using System.Collections.Generic;

namespace RawRabbitSubscriber
{
    public class RabbitEventListener
    {
        private readonly IBusClient busClient;
        private readonly IServiceProvider serviceProvider;

        public RabbitEventListener(
            IBusClient busClient,
            IServiceProvider serviceProvider)
        {
            this.busClient = busClient;
            this.serviceProvider = serviceProvider;
        }

        public void ListenTo(List<Type> eventsToSubscribe)
        {
            foreach (var evtType in eventsToSubscribe)
            {
                //add check if is INotification
                this.GetType()
                    .GetMethod("Subscribe", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .MakeGenericMethod(evtType)
                    .Invoke(this, new object[] { });
            }
        }

        private void Subscribe<T>() where T : INotification
        {
            //TODO: move exchange name and queue prefix to cfg
            this.busClient.SubscribeAsync<T>(
                async (msg) =>
                {
                    //add logging
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var internalBus = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await internalBus.Publish(msg);
                    }
                },
                cfg => cfg.UseSubscribeConfiguration(
                    c => c
                        .OnDeclaredExchange(e => e
                            .WithName("rabbit-test")
                            .WithType(RawRabbit.Configuration.Exchange.ExchangeType.Topic)
                            .WithArgument("key", typeof(T).Name.ToLower()))
                        .FromDeclaredQueue(q => q.WithName("subscriber-service-" + typeof(T).Name)))
            );
        }
    }
}
