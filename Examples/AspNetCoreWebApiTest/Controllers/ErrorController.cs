using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreWebApiTest.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("test-exception")]
        public IActionResult TestException()
        {
            //var testException = new HttpResponseException();
            //testException.Status = 404;
            //testException.Value = new string("Test Not Found Exception");
            //throw testException;

            var testException = new Exception("Test Unhandled Exception");

            throw testException;
        }

        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
