using System;
using CloudNative.CloudEvents;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AspNetCoreCloudEventsTest.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class CloudEventController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> SendCloudEvent()
        {
            var source = "urn:example-com:mysource:abc";
            var type = "com.example.myevent";
            var cloudEvent = new CloudEvent(type, new Uri(source))
            {
                DataContentType = new ContentType(MediaTypeNames.Application.Json),
                Data = JsonConvert.SerializeObject("hey there!")
            };

            var content = new CloudEventContent(cloudEvent, ContentMode.Structured, new JsonEventFormatter());

            var httpClient = new HttpClient();
            var url = new Uri("http://localhost:5000/api/events/receive");
            var result = await httpClient.PostAsync(url, content);
            return Ok(result);
        }

        [HttpPost("receive")]
        public ActionResult<IEnumerable<string>> ReceiveCloudEvent([FromBody] CloudEvent cloudEvent)
        {
            return Ok($"Received event with ID {cloudEvent.Id}, attributes: {JsonConvert.SerializeObject(cloudEvent.GetAttributes())}");
        }
    }
}
