using System.Collections.Generic;
using System.Linq;
using LibraryData;
using LibraryData.Models;

namespace LibraryService
{
    public class BookService : IBook
    {
        // need to give the class a constructor that takes a dbContext.
        // save that off into a private field where we can store the dbContext.
        // then, implment IBook interface.

        private readonly LibraryDbContext _context; // private field to store the context.

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Book newBook)
        {
            _context.Add(newBook);
            _context.SaveChanges();
        }

        public Book Get(int id)
        {
            return _context.Books.FirstOrDefault(book => book.Id == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books;
        }

        public IEnumerable<Book> GetByAuthor(string author)
        {
            return _context.Books.Where(a => a.Author == author);
        }

        public IEnumerable<Book> GetByISBN(string isbn)
        {
            return _context.Books.Where(a => a.ISBN == isbn);
        }
    }
}