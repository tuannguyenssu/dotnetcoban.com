using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace AspNetCoreMicroservicesTest.IntegrationEvent
{
    public interface IMessage
    {
        Guid Id { get; }
        Guid CorrelationId { get; }
        DateTime? CreatedDate { get; }

    }
    public interface EventCreated : CorrelatedBy<Guid>
    {
    }

}
