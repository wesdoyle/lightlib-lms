using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface IBook
    {
        IEnumerable<Book> GetAll();
        IEnumerable<Book> GetByAuthor(string author);
        IEnumerable<Book> GetByISBN(string isbn);
        Book Get(int id);
        void Add(Book newBook);
    }
}