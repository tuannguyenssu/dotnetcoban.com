using AspNetCoreCouchbaseTest.Configuration;
using AspNetCoreCouchbaseTest.Models;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace AspNetCoreCouchbaseTest.Extension
{
    public static class ServiceCollectionExtension
    {
        private const string CouchbaseSectionName = "Couchbase";
        public static IServiceCollection AddCustomCouchbase(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using (var scope = resolver.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();

                var couchbaseOption = new CouchbaseOptions();
                config.Bind(CouchbaseSectionName, couchbaseOption);

                services.AddSingleton(couchbaseOption);

                var servers = new List<Uri>();
                foreach (var server in couchbaseOption.Servers)
                {
                    servers.Add(new Uri(server));
                }
                var cluster = new Cluster(new ClientConfiguration
                {
                    Servers = servers
                });
                cluster.Authenticate(new PasswordAuthenticator(couchbaseOption.UserName, couchbaseOption.Password));
                services.AddSingleton(x => cluster.OpenBucket(couchbaseOption.DefaultBucket));
                services.AddTransient<ProfileRepository>();
                return services;
            }

        }
    }
}
