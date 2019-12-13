using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CAPEventBusTest.Controllers
{
    public class PublishController : Controller
    {
        private readonly ICapPublisher _capBus;

        public PublishController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        [Route("~/send")]
        public IActionResult SendMessage()
        {
            _capBus.Publish("test.show.time", DateTime.Now);

            return Ok();
        }
    }
}