using LibraryData;
using System.Collections.Generic;
using LibraryData.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace LibraryService
{
    public class LibraryBranchService : ILibraryBranch
    {
        private LibraryDbContext _context; // private field to store the context.

        public LibraryBranchService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(LibraryBranch newBranch)
        {
            _context.Add(newBranch);
            _context.SaveChanges();
        }

        public LibraryBranch Get(int branchId)
        {
            return _context.LibraryBranches
                .Include(b=>b.Patrons)
                .Include(b=>b.LibraryAssets)
                .FirstOrDefault(p => p.Id == branchId);
        }

        public IEnumerable<LibraryBranch> GetAll()
        {
            return _context.LibraryBranches.Include(a=>a.Patrons).Include(a=>a.LibraryAssets);
        }

        public int GetAssetCount(IEnumerable<LibraryAsset> libraryAssets)
        {
            if (libraryAssets == null)
            {
                return 0;
            }

            else return libraryAssets.Count();
        }

        public IEnumerable<LibraryAsset> GetAssets(int branchId)
        {
            return _context.LibraryBranches.Include(a=>a.LibraryAssets)
                .FirstOrDefault(b => b.Id == branchId)
                .LibraryAssets;
        }

        public decimal GetAssetsValue(int branchId)
        {
            var assetsValue = GetAssets(branchId).Select(a=>a.Cost);
            return assetsValue.Sum();
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context.BranchHours.Where(a => a.Branch.Id == branchId);

            var displayHours = 
                DataHelpers.HumanizeBusinessHours(hours);

            return displayHours;
        }

        public int GetPatronCount(IEnumerable<Patron> patrons)
        {
            if(patrons == null)
            {
                return 0;
            }

            else return patrons.Count();
        }

        public IEnumerable<Patron> GetPatrons(int branchId)
        {
            return _context.LibraryBranches.Include(a=>a.Patrons).FirstOrDefault(b => b.Id == branchId).Patrons;
        }

        public bool IsBranchOpen(int branchId)
        {
            var currentTime = DateTime.Now;
            return true; 
        }
    }
}
