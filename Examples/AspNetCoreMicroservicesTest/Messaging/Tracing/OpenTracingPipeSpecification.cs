using System.Collections.Generic;
using System.Linq;
using GreenPipes;
using MassTransit;

namespace AspNetCoreMicroservicesTest.Messaging.Tracing
{
    public class OpenTracingPipeSpecification : IPipeSpecification<ConsumeContext>, IPipeSpecification<PublishContext>
    {
        public void Apply(IPipeBuilder<ConsumeContext> builder)
        {
            // builder.AddFilter(new OpenTracingConsumeFilter());
        }

        public void Apply(IPipeBuilder<PublishContext> builder)
        {
            builder.AddFilter(new OpenTracingPublishFilter());
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
