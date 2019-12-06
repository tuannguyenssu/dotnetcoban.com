using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCorePermissionAuthorizationTest
{
    public class FakeUserLoginMiddleware
    {
        private readonly RequestDelegate _next;


        public FakeUserLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //if (httpContext.Request.Path.StartsWithSegments(new PathString("/user")))
            //{
            //    // Giả lập việc đăng nhập

            //    var signInManager = httpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
            //    var userManager = httpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            //    var loginUser = await userManager.FindByNameAsync("admin");
            //    //var loginUser = await userManager.FindByNameAsync("user");
            //    var result = await signInManager.PasswordSignInAsync(loginUser, "1234", false, false);
            //    if (!result.Succeeded)
            //    {
            //        throw new Exception("Login failed");
            //    }
            //}


            var signInManager = httpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
            var userManager = httpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            var loginUser = await userManager.FindByNameAsync("admin");
            //var loginUser = await userManager.FindByNameAsync("user");
            var result = await signInManager.PasswordSignInAsync(loginUser, "1234", false, false);
            if (!result.Succeeded)
            {
                throw new Exception("Login failed");
            }

            var token = await userManager.GenerateChangeEmailTokenAsync(loginUser, loginUser.Email);
            await _next(httpContext);
        }
    }
}