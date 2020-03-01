using AspNetCoreMongoTest.Configuration;
using AspNetCoreMongoTest.Models;
using MongoDB.Driver;

namespace AspNetCoreMongoTest
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoOptions options)
        {
            var client = new MongoClient(options.ConnectionString);
            _database = client.GetDatabase(options.DefaultDatabase);
        }

        public IMongoCollection<Book> Books => _database.GetCollection<Book>(nameof(Book));

    }
}
