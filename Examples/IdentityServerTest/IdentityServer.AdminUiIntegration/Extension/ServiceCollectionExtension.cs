using IdentityExpress.Identity;
using IdentityServer.AdminUiIntegration.Customized;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace IdentityServer.AdminUiIntegration.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services)
        {
            services.AddCustomIdentityServerStorage();
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            return services;
        }

        private static void AddCustomIdentityServerStorage(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using var scope = resolver.CreateScope();
            var config = scope.ServiceProvider.GetService<IConfiguration>();
            string connectionString = config.GetConnectionString("DefaultConnection");

            Action<DbContextOptionsBuilder> identityBuilder;
            Action<DbContextOptionsBuilder> identityServerBuilder;

            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            switch (Program.UseDatabaseType)
            {
                case DatabaseType.SqlServer:
                    identityBuilder = x => x.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationAssembly));
                    identityServerBuilder = x => x.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationAssembly));
                    break;
                case DatabaseType.SqLite:
                    connectionString = "Data Source=IdentityServer.db;";
                    identityBuilder = x => x.UseSqlite(connectionString, options => options.MigrationsAssembly(migrationAssembly));
                    identityServerBuilder = x => x.UseSqlite(connectionString, options => options.MigrationsAssembly(migrationAssembly));
                    break;
                default:
                    const string databaseName = "IdentityServerDb";
                    identityBuilder = x => x.UseInMemoryDatabase(databaseName);
                    identityServerBuilder = x => x.UseInMemoryDatabase(databaseName);
                    break;
            }

            services.AddDbContext<ApplicationDbContext>(identityBuilder);

            services.AddIdentity<AppUser, IdentityExpressRole>(options =>
                {
                    // Basic built in validations
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.UserInteraction = new UserInteractionOptions
                    {
                        LogoutUrl = "/Account/Logout",
                        LoginUrl = "/Account/Login",
                        LoginReturnUrlParameter = "returnUrl"
                    };
                })
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options => { options.ConfigureDbContext = identityServerBuilder; })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = identityServerBuilder;

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<AppUser>()
                .AddJwtBearerClientAuthentication()
                .AddDeveloperSigningCredential();
        }
    }
}
