namespace AspNetCoreCassandraTest.Configuration
{
    public class CassandraOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DefaultKeySpace { get; set; }
    }
}
