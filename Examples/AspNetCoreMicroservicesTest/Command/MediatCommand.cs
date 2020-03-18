using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreMicroservicesTest.IntegrationEvent;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMicroservicesTest.Command
{
    public class MediatCommand : IRequest<bool>
    {
    }

    public class MediatCommandHandler : IRequestHandler<MediatCommand, bool>
    {
        private readonly IBus _bus;
        private readonly ILogger<MediatCommandHandler> _logger;

        public MediatCommandHandler(IBus bus, ILogger<MediatCommandHandler> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task<bool> Handle(MediatCommand request, CancellationToken cancellationToken)
        {
            await _bus.Publish<EventCreated>(new string("EventCreated"), cancellationToken);
            return await Task.FromResult(true);
        }
    }
}
