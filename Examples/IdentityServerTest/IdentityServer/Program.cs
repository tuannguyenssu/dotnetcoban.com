using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    InitializeIdentityServer(services);
                }
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.Listen(IPAddress.Any, 5000);
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                });
        }

        private static void InitializeIdentityServer(IServiceProvider provider)
        {
            // Uncomment when connecting to the real database
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
            var user = userManager.FindByNameAsync("tuannguyen").Result;
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "tuannguyen"
                };
                var result = userManager.CreateAsync(user, "P@ssw0rd").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                user = userManager.FindByNameAsync("tuannguyen").Result;

                result = userManager.AddClaimsAsync(user, new Claim[]{
                new Claim(JwtClaimTypes.Name, "Tuan Nguyen"),
                new Claim(JwtClaimTypes.GivenName, "Tuan"),
                new Claim(JwtClaimTypes.FamilyName, "Nguyen"),
                new Claim(JwtClaimTypes.Email, "admin@dotnetcoban.com"),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, "https://dotnetcoban.com"),
                new Claim(JwtClaimTypes.Address, @"{ 'country': 'Vietnam' }",
                    IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
            }).Result;
            }
        }
    }

}
