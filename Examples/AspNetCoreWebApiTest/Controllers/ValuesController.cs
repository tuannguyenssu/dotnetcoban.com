using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiTest.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult Get()
        {
            var values = new List<string>()
            {
                "value 1",
                "value 2"
            };

            return Ok(values);
        }
    }
}
