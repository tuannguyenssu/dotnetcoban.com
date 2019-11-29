using AspNetCoreAuthorizationTest.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreAuthorizationTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthorization(options => 
            {
                // Role-based
                options.AddPolicy(RoleBasedPolicies.IsAdmin, RoleBasedPolicies.IsAdminPolicy());
                options.AddPolicy(RoleBasedPolicies.IsUser, RoleBasedPolicies.IsUserPolicy());

                // Policy-based
                options.AddPolicy(PolicyBasedPolicies.IsAdmin, PolicyBasedPolicies.IsAdminPolicy());
                options.AddPolicy(PolicyBasedPolicies.IsUser, PolicyBasedPolicies.IsUserPolicy());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<FakeRoleBasedMiddleware>();

            app.UseMiddleware<FakePolicyBasedMiddleware>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("AspNetCore Authorization Test");
                });

                endpoints.MapControllers();
            });
        }
    }
}
