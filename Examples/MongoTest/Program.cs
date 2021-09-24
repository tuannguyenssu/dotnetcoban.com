using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
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

            var id = "60f69c3d88c7213cf12e5bd6";
            var teacher = new TeacherDao()
            {
                TeacherId = ObjectId.Parse(id).ToString(),
                Name = "Tuan Nguyen 1",
                CreatedTime = DateTime.Now
            };

            //await context.GetStudents().InsertOneAsync(student);
            await context.GetTeachers().InsertOneAsync(teacher);

            //var items = await context.GetStudents().Find(Builders<StudentDao>.Filter.Empty).ToListAsync();
            var items = await context.GetTeachers().Find(Builders<TeacherDao>.Filter.Eq(x => x.TeacherId, id)).ToListAsync();

        }
    }
}
