using LibraryData;
using LibraryData.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryService
{
    public class BookService : IBook
    {
        // need to give the class a constructor that takes a dbContext.
        // save that off into a private field where we can store the dbContext.
        // then, implment IBook interface.

        private LibraryDbContext _context; // private field to store the context.

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Book newBook)
        {
            _context.Add(newBook);
            _context.SaveChanges(); 
            // Could move this elsewhere to group multiple add or update operations
            // into a single transaction, you'll only want to call savechanges once
            // after all work is complete.
        }

        public Book Get(int assetId)
        {
            return _context.Books.FirstOrDefault(book => book.Id == assetId);
        }

        // Maybe want this to be IQueryable to allow other parts
        // of the software to perform additional operations for paging, filtering, joining, etc.
        // in linq.
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
