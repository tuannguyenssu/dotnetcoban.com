using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AspNetCoreMicroservicesTest.Queries
{
    public class MediatQuery : IRequest<bool>
    {
    }

    public class MediatQueryHandler : IRequestHandler<MediatQuery, bool>
    {
        public async Task<bool> Handle(MediatQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}
