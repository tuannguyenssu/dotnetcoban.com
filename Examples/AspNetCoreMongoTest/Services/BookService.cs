using System.Collections.Generic;
using AspNetCoreMongoTest.Models;
using MongoDB.Driver;

namespace AspNetCoreMongoTest.Services
{
    public class BookService
    {
        private readonly MongoDbContext _context;
        public BookService(MongoDbContext context)
        {
            _context = context;
        }

        public List<Book> Get() => _context.Books.Find(book => true).ToList();

        public Book Get(string id) => _context.Books.Find(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _context.Books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) => _context.Books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) => _context.Books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) => _context.Books.DeleteOne(book => book.Id == id);
    }
}