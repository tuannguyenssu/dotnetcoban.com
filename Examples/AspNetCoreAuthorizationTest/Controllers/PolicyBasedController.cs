using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAuthorizationTest.Controllers
{
    [ApiController]
    [Route("test-policy")]
    public class PolicyBasedController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Policy = PolicyBasedPolicies.IsAdmin)]
        public ActionResult<IEnumerable<string>> GetAdmin()
        {
            if (!User.HasClaim(c => c.Type == PolicyBasedPolicies.ClaimDiploma))
            {
                HttpContext.Response.WriteAsync("Unauthorized");
                return Unauthorized();
            }
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Policy = PolicyBasedPolicies.IsUser)]
        public ActionResult<IEnumerable<string>> GetUser()
        {
            if (!User.HasClaim(c => c.Type == PolicyBasedPolicies.ClaimCmnd)) return Unauthorized();
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}