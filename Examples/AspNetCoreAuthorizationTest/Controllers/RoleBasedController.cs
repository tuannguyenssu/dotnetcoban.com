using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreAuthorizationTest.Controllers
{
    [ApiController]
    [Route("test-role")]
    public class RoleBasedController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<string>> GetAdmin()
        {
            if (!User.IsInRole(RoleBasedPolicies.RoleAdmin))
            {
                HttpContext.Response.WriteAsync("Unauthorized");
                return Unauthorized();
            }
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "User, Admin")]
        public ActionResult<IEnumerable<string>> GetUser()
        {
            if (!User.IsInRole(RoleBasedPolicies.RoleUser)) return Unauthorized();
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
