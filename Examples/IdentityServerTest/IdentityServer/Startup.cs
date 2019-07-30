using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //SetupIdentityServerWithIdentityServerInMemory(services);

            SetupIdentityServerWithSQLInMemory(services);

            //SetupIdentityServerWithSQLServer(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }

        // Setup Identity Server with SQL InMemory
        private void SetupIdentityServerWithSQLServer(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            // this adds the config data from DB (clients, resources)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = opt =>
                {
                    opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                };
            })
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = opt =>
                {
                    opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                };

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddJwtBearerClientAuthentication()
            .AddDeveloperSigningCredential();
        }

        // Setup Identity Server with SQL InMemory
        private void SetupIdentityServerWithSQLInMemory(IServiceCollection services)
        {
            string dbName = "IdentityServerDb";
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseInMemoryDatabase(dbName);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            // this adds the config data from DB (clients, resources)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = opt =>
                {
                    opt.UseInMemoryDatabase("IdentityServerDb");
                };
            })
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = opt =>
                {
                    opt.UseInMemoryDatabase("IdentityServerDb");
                };

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddJwtBearerClientAuthentication()
            .AddDeveloperSigningCredential();
        }

        // Quick test IdentityServer (Every data is saved in memory)
        private void SetupIdentityServerWithIdentityServerInMemory(IServiceCollection services)
        {
            services.AddIdentityServer(
                          options =>
                          {
                              options.Events.RaiseErrorEvents = true;
                              options.Events.RaiseFailureEvents = true;
                              options.Events.RaiseInformationEvents = true;
                              options.Events.RaiseSuccessEvents = true;
                          }
                )
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddJwtBearerClientAuthentication()
                .AddTestUsers(Config.GetTestUsers())
                .AddDeveloperSigningCredential();
        }
    }
}
