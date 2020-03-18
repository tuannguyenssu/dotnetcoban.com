using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMicroservicesTest.Command;
using AspNetCoreMicroservicesTest.CorrelationId;
using AspNetCoreMicroservicesTest.IntegrationEvent;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroservicesTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly MediatR.IMediator _mediator;

        public ClientsController(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> SendEvent()
        {
            await _mediator.Send(new MediatCommand());
            return Ok();
        }
    }
}
