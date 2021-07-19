using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new MongoDbOptions()
            {
                ConnectionString = "mongodb://ba:123%40123a@10.0.10.21:28017/?authSource=admin",
                Database = "TestDb"
            };

            var context = new MongoDbContext(options);
            var student = new StudentDao()
            {
                TeacherId = "GV1",
                Name = "Tuan Nguyen 1",
                Tags = new List<Tag>()
                {
                    Tag.Tag1,
                    Tag.Tag2
                },
                CreatedTime = DateTime.Now
            };

            await context.GetStudents().InsertOneAsync(student);

            var items = await context.GetStudents().Find(Builders<StudentDao>.Filter.Empty).ToListAsync();

        }
    }
}
