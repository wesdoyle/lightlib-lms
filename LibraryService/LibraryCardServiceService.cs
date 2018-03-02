using System.Collections.Generic;
using System.Linq;
using LibraryData;
using LibraryData.Models;

namespace LibraryService
{
    public class LibraryCardServiceService : ILibraryCardService
    {
        private readonly LibraryDbContext _context;

        public LibraryCardServiceService(LibraryDbContext context)
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

        IEnumerable<LibraryCard> ILibraryCardService.GetAll()
        {
            return _context.LibraryCards;
        }
    }
}