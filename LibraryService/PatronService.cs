using LibraryData;
using System.Collections.Generic;
using LibraryData.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace LibraryService
{
    public class PatronService : IPatron
    {
        private LibraryDbContext _context; // private field to store the context.

        public PatronService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Patron newPatron)
        {
            _context.Add(newPatron);
            _context.SaveChanges();
        }

        public Patron Get(int id)
        {
            return _context.Patrons
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Patron> GetAll()
        {
            return _context.Patrons
                .Include(a=>a.LibraryCard)
                .Include(a=>a.HomeLibraryBranch);
            // Eager load this data.
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronId)
        {
            var cardId = _context.Patrons
                .Include(a=>a.LibraryCard)
                .FirstOrDefault(a => a.Id == patronId)
                .LibraryCard.Id;

            return _context.CheckoutHistories
                .Include(a=>a.LibraryCard)
                .Include(a=>a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a=>a.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int id)
        {
            var patronCardId = Get(id).LibraryCard.Id;
            return _context.Checkouts
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(v => v.LibraryCard.Id == patronCardId);
        }

        public IEnumerable<Hold> GetHolds(int patronId)
        {
            var cardId = _context.Patrons
                .Include(a=>a.LibraryCard)
                .FirstOrDefault(a => a.Id == patronId)
                .LibraryCard.Id;

            return _context.Holds
                .Include(a=>a.LibraryCard)
                .Include(a=>a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.HoldPlaced);
        }
    }
}
