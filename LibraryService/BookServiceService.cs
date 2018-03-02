using System.Collections.Generic;
using System.Linq;
using LibraryData;
using LibraryData.Models;

namespace LibraryService
{
    public class BookServiceService : IBookService
    {
        private readonly LibraryDbContext _context;

        public BookServiceService(LibraryDbContext context)
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

        public IEnumerable<Book> GetByIsbn(string isbn)
        {
            return _context.Books.Where(a => a.ISBN == isbn);
        }
    }
}