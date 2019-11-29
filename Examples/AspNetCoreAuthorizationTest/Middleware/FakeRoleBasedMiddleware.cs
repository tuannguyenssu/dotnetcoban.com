using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreAuthorizationTest.Middleware
{
    public class FakeRoleBasedMiddleware
    {
        private readonly RequestDelegate _next;

        public FakeRoleBasedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Tạo các claims cần thiết
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Role, "Admin")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Giả lập việc đăng nhập
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Thêm các thông tin claims vào context
            httpContext.User.AddIdentity(claimsIdentity);

            await _next(httpContext);
        }
    }
}
