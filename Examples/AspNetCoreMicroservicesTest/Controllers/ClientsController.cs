using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IBusControl _bus;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public ClientsController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> SendEvent()
        {
            await Task.CompletedTask;
            await _bus.Publish<EventCreated>(new
            {
                _correlationContextAccessor?.CorrelationContext?.CorrelationId
            });
            return Ok();
        }
    }
}
