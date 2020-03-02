using System;
using AspNetCoreCassandraTest.Configuration;
using AspNetCoreCassandraTest.Models;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreCassandraTest.Extensions
{
    public static class ServiceCollectionExtension
    {

        private const string CassandraSectionName = "Cassandra";
        public static IServiceCollection AddCassandra(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using (var scope = resolver.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();

                var cassandraOptions = new CassandraOptions();
                config.Bind(CassandraSectionName, cassandraOptions);

                services.AddSingleton(cassandraOptions);

                var poolOptions = new PoolingOptions();
                poolOptions.SetMaxRequestsPerConnection(Int32.MaxValue);
                var cluster = Cluster.Builder()
                    .AddContactPoints(cassandraOptions.Host)
                    .WithPort(cassandraOptions.Port)
                    .WithAuthProvider(new PlainTextAuthProvider(cassandraOptions.UserName, cassandraOptions.Password))
                    .WithDefaultKeyspace(cassandraOptions.DefaultKeySpace)
                    .WithPoolingOptions(poolOptions)
                    .Build();

                services.AddSingleton(x => cluster.Connect());
                services.AddTransient<BookRepository>();
                return services;
            }
        }
    }
}
