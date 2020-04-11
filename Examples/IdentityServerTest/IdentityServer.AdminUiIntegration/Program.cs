using IdentityServer.AdminUiIntegration.Customized;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
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

namespace IdentityServer.AdminUiIntegration
{
    public class Program
    {
        public static DatabaseType UseDatabaseType = DatabaseType.InMemory;
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
            if (UseDatabaseType != DatabaseType.InMemory)
            {
                provider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                provider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                provider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            }

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

            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var user = userManager.FindByNameAsync("admin").Result;
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = "admin",
                    FullName = "Admin",
                    Email = "admin@dotnetcoban.com",
                    Role = "admin"

                };
                var result = userManager.CreateAsync(user, "admin").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                //user = userManager.FindByNameAsync("admin").Result;

                //result = userManager.AddClaimsAsync(user, new Claim[]{
                //new Claim(JwtClaimTypes.Role, "admin"),
                //new Claim(JwtClaimTypes.Name, user.FullName),
                //new Claim(JwtClaimTypes.Email, user.Email),
                //new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                //}).Result;
            }

            user = userManager.FindByNameAsync("member").Result;
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = "member",
                    FullName = "Member",
                    Email = "member@dotnetcoban.com",
                    Role = "member"
                };
                var result = userManager.CreateAsync(user, "member").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                //user = userManager.FindByNameAsync("member").Result;

                //result = userManager.AddClaimsAsync(user, new Claim[]{
                //    new Claim(JwtClaimTypes.Role, "member"),
                //    new Claim(JwtClaimTypes.Name, user.FullName),
                //    new Claim(JwtClaimTypes.Email, user.Email),
                //    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                //}).Result;
            }
        }
    }
}
