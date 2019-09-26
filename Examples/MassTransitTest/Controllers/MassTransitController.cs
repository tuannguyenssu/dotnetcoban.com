using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MassTransit;

namespace MassTransitTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MassTransitController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<MassTransitController> _logger;

        public MassTransitController(IBus bus, ILogger<MassTransitController> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            string currentTime = DateTime.Now.ToString();
            _logger.LogInformation($"Sent order data: {currentTime}");
            await _bus.Publish<Order>(
            new
            {
                Data = currentTime
            });
            return currentTime;
        }
    }
}
