using System;
using AspNetCoreMicroservicesTest.Guard;

namespace AspNetCoreMicroservicesTest.CorrelationId
{
    public class CorrelationContext
    {
        internal CorrelationContext(Guid correlationId, String header)
        {
            correlationId.NotNullOrEmpty();
            header.NotNullOrEmpty();

            CorrelationId = correlationId;
            Header = header;
        }
        public Guid CorrelationId { get; }

        public String Header { get; }
    }
}
