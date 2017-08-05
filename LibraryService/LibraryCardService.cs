using LibraryData;
using System.Collections.Generic;
using LibraryData.Models;
using System.Linq;

namespace LibraryService
{
    public class LibraryCardService: ILibraryCard
    {
        private LibraryDbContext _context; // private field to store the context.

        public LibraryCardService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(LibraryCard newLibraryCard)
        {
            _context.Add(newLibraryCard);
            _context.SaveChanges();
        }

        public LibraryCard Get(int cardId)
        {
            return _context.LibraryCards.FirstOrDefault(p => p.Id == cardId);
        }

        IEnumerable<LibraryCard> ILibraryCard.GetAll()
        {
            return _context.LibraryCards;
        }
    }
}
