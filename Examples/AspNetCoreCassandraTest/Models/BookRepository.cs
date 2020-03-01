using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;

namespace AspNetCoreCassandraTest.Models
{
    public class BookRepository
    {
        private readonly ISession _session;

        public BookRepository(ISession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            var mapper = new Mapper(_session);
            var cql = "select * from books";
            return await mapper.FetchAsync<Book>(cql);
        }
    }
}
