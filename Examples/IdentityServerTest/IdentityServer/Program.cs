using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                InitializeIdentityServer(services);
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitializeIdentityServer(IServiceProvider provider)
        {
            //provider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            //provider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            //provider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

            var context = provider.GetRequiredService<ConfigurationDbContext>();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApis())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var bob = userManager.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob"
                };
                var result = userManager.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                bob = userManager.FindByNameAsync("bob").Result;

                result = userManager.AddClaimsAsync(bob, new Claim[]{
                new Claim(JwtClaimTypes.Name, "Alice Bob"),
                new Claim(JwtClaimTypes.GivenName, "Bob"),
                new Claim(JwtClaimTypes.FamilyName, "Alice"),
                new Claim(JwtClaimTypes.Email, "bob@blog.com"),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, "https://example.com"),
                new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'localhost 10', 'postal_code': 11146, 'country': 'Greece' }",
                    IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
            }).Result;
            }
        }
    }

}

//https://blog.georgekosmidis.net/2019/02/08/identityserver4-asp-dotnet-core-api-and-a-client-with-username-password/
//https://chsakell.com/2019/03/11/asp-net-core-identity-series-oauth-2-0-openid-connect-identityserver/

//DROP DATABASE IdentityServerDb
//Remove-Migration -Context PersistedGrantDbContext
//Remove-Migration -Context ConfigurationDbContext
//Remove-Migration -Context ApplicationDbContext

//Add-Migration InitApplicationDbContext -Context ApplicationDbContext
//Add-Migration InitPersistedGrantDbContext -Context PersistedGrantDbContext
//Add-Migration InitConfigurationDbContext -Context ConfigurationDbContext