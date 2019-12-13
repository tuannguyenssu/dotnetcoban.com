using MongoDB.Driver;
using MongoDBTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBTest.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "BookstoreDb";
        private const string BooksCollectionName = "Books";
        public BookService()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);

            _books = database.GetCollection<Book>(BooksCollectionName);
        }

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) => 
            _books.DeleteOne(book => book.Id == id);
    }
}