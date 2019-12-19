using AspNetCoreStarterTemplateTest.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspNetCoreStarterTemplateTest.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ApplicationUser>> Get()
        {
            var persons = GenFu.GenFu.ListOf<ApplicationUser>(25);
            return persons;
        }
    }
}
