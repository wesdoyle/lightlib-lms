using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Data.Models;
using Library.Models.DTOs;
using Library.Service.Helpers;
using Library.Service.Interfaces;
using Library.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Service
{
    public class LibraryBranchService : ILibraryBranchService
    {
        private readonly LibraryDbContext _context;

        public LibraryBranchService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(LibraryBranch newBranch)
        {
            _context.Add(newBranch);
            _context.SaveChanges();
        }

        Task<ServiceResult<List<string>>> ILibraryBranchService.GetBranchHours(int branchId) {
            throw new System.NotImplementedException();
        }

        Task<ServiceResult<LibraryBranchDto>> ILibraryBranchService.Get(int branchId) {
            throw new System.NotImplementedException();
        }

        public Task<ServiceResult<int>> Add(LibraryBranchDto newBranch) {
            throw new System.NotImplementedException();
        }

        Task<ServiceResult<bool>> ILibraryBranchService.IsBranchOpen(int branchId) {
            throw new System.NotImplementedException();
        }

        Task<ServiceResult<int>> ILibraryBranchService.GetAssetCount(int branchId) {
            throw new System.NotImplementedException();
        }

        Task<ServiceResult<int>> ILibraryBranchService.GetPatronCount(int branchId) {
            throw new System.NotImplementedException();
        }

        Task<ServiceResult<decimal>> ILibraryBranchService.GetAssetsValue(int id) {
            throw new System.NotImplementedException();
        }

        public LibraryBranch Get(int branchId)
        {
            return _context.LibraryBranches
                .Include(b => b.Patrons)
                .Include(b => b.LibraryAssets)
                .FirstOrDefault(p => p.Id == branchId);
        }

        public IEnumerable<LibraryBranch> GetAll()
        {
            return _context.LibraryBranches.Include(a => a.Patrons).Include(a => a.LibraryAssets);
        }

        public int GetAssetCount(int branchId)
        {
            return Get(branchId).LibraryAssets.Count();
        }

        public IEnumerable<LibraryAsset> GetAssets(int branchId)
        {
            return _context.LibraryBranches.Include(a => a.LibraryAssets)
                .First(b => b.Id == branchId).LibraryAssets;
        }

        public decimal GetAssetsValue(int branchId)
        {
            var assetsValue = GetAssets(branchId).Select(a => a.Cost);
            return assetsValue.Sum();
        }

        public Task<PagedServiceResult<LibraryBranchDto>> GetAll(int page, int perPage) {
            throw new System.NotImplementedException();
        }

        public Task<PagedServiceResult<PatronDto>> GetPatrons(int page, int perPage) {
            throw new System.NotImplementedException();
        }

        public Task<PagedServiceResult<LibraryAssetDto>> GetAssets(int page, int perPage) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context.BranchHours.Where(a => a.Branch.Id == branchId);

            var displayHours =
                DataHelpers.HumanizeBusinessHours(hours);

            return displayHours;
        }

        public int GetPatronCount(int branchId)
        {
            return Get(branchId).Patrons.Count();
        }

        public IEnumerable<Patron> GetPatrons(int branchId)
        {
            return _context.LibraryBranches.Include(a => a.Patrons).First(b => b.Id == branchId).Patrons;
        }

        //TODO: Implement 
        public bool IsBranchOpen(int branchId)
        {
            return true;
        }
    }
}