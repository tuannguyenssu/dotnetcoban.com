namespace AspNetCoreMongoTest.Configuration
{
    public interface IMongoOptions
    {
        public string ConnectionString { get; set; }
        public string DefaultDatabase { get; set; }
    }
    public class MongoOptions : IMongoOptions
    {
        public string ConnectionString { get; set; }
        public string DefaultDatabase { get; set; }
    }
}
