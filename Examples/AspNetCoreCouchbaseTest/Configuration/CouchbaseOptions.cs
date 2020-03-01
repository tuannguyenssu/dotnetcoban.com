namespace AspNetCoreCouchbaseTest.Configuration
{
    public class CouchbaseOptions
    {
        public string[] Servers { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DefaultBucket { get; set; }
    }
}
