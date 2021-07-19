using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace MongoTest
{
    public enum Tag
    {
        Unknown,
        Tag1,
        Tag2
    }

    public class TeacherDao
    {
        public string TeacherId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public class StudentDao
    {
        //[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        //[BsonRepresentation(BsonType.ObjectId)]
        //[BsonIgnoreIfDefault]
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string TeacherId { get; set; }
        public string Name { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public DateTime CreatedTime { get; set; }
    }

    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            var pack = new ConventionPack
            {
                //new CamelCaseElementNameConvention(), // In thường chữ cái đầu
                new IgnoreExtraElementsConvention(true),
                new StringIdStoredAsObjectIdConvention(),
                //new EnumRepresentationConvention(BsonType.String), // Lưu enum dưới dạng các con số
                new IgnoreIfNullConvention(true),
            };
            ConventionRegistry.Register("Conventions", pack, t => true);

            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterIdGenerator(typeof(Guid), AscendingGuidGenerator.Instance);
            //BsonSerializer.RegisterSerializer(new EnumSerializer<Tag>(BsonType.String)); //Lưu enum dưới dạng string

            BsonClassMap.RegisterClassMap<StudentDao>(map =>
            {
                map.AutoMap();
                //map.SetIdMember(map.GetMemberMap(x => x.StudentId));
                //map.MapIdMember(x => x.StudentId).SetIdGenerator(StringObjectIdGenerator.Instance);
                map.MapProperty(c => c.StudentId).SetIdGenerator(StringObjectIdGenerator.Instance).SetSerializer(new StringSerializer(BsonType.ObjectId));
                map.MapMember(x => x.Name).SetDefaultValue(string.Empty);
            });
        }
    }

    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }

    public static class CollectionNames
    {
        public static string Student = "Student";
    }

    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(MongoDbOptions options)
        {
            MongoDbPersistence.Configure();
            var client = new MongoClient(options.ConnectionString);
            _database = client.GetDatabase(options.Database);
        }

        public IMongoCollection<StudentDao> GetStudents()
        {
            return _database.GetCollection<StudentDao>(CollectionNames.Student);
        }
    }
}