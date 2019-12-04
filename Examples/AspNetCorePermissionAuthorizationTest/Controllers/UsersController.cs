using System.Collections.Generic;
using System.Linq;
using AspNetCorePermissionAuthorizationTest.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCorePermissionAuthorizationTest.Controllers
{
    [Authorize]
    [Route("user")]
    public class UsersController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [Authorize(Permissions.Users.View)]
        [Route("view")]
        public ActionResult ViewDashboard()
        {
            return Ok(Permissions.Users.View);
        }

        [Authorize(Permissions.Users.Create)]
        [Route("create")]
        public ActionResult CreateDashboard()
        {
            return Ok(Permissions.Users.Create);
        }
    }
}
