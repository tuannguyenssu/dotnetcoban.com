using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace CAPEventBusTest.Controllers
{
    public class ConsumerController : Controller
    {
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }

        [NonAction]
        [CapSubscribe("test.show.time")]
        public void ReceiveMessage(DateTime time)
        {
            _logger.LogInformation("message time is:" + time);
        }
    }
}