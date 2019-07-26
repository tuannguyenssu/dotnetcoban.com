using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
