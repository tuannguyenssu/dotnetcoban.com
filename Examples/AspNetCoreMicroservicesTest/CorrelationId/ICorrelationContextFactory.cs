using System;

namespace AspNetCoreMicroservicesTest.CorrelationId
{
    public interface ICorrelationContextFactory
    {
        CorrelationContext Create(Guid correlationId, String header);
        void Dispose();
    }
}
