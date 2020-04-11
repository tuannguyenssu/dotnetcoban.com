using IdentityServer.Customized;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace IdentityServer.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services)
        {
            services.AddCustomAuthentication();
            services.AddCustomIdentityServerStorage();
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            return services;
        }

        private static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "133118749581-j5ge4p2028lqmc2p815fr7unc02gj54j.apps.googleusercontent.com";
                    options.ClientSecret = "AswzkKMScTfmmVUDy4ybXxmv";
                })
                .AddFacebook("Facebook", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "463588604159289";
                    options.ClientSecret = "3edbc567b8eb0b1eb97dc957ba49d820";
                })
                .AddZalo("Zalo", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "3731698280163837939";
                    options.ClientSecret = "4pIvbXavvqEXUuiDL3ZS";
                });
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

            services.AddIdentity<AppUser, IdentityRole>(options =>
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
