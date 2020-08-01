using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data;
using Library.Data.Models;
using Library.Models.DTOs;
using Library.Service.Interfaces;
using Library.Service.Models;
using Microsoft.EntityFrameworkCore;
using static Library.Service.Helpers.DataHelpers;

namespace Library.Service {
    /// <summary>
    /// Handles business logic related to Library Branches 
    /// </summary>
    public class LibraryBranchService : ILibraryBranchService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<LibraryBranch> _paginator;

        public LibraryBranchService(
            LibraryDbContext context, 
            IMapper mapper, 
            IPaginator<LibraryBranch> paginator) {
            _context = context;
            _mapper = mapper;
            _paginator = paginator;
        }

        /// <summary>
        /// Creates a new Library Branch
        /// </summary>
        /// <param name="newBranch"></param>
        public async Task<ServiceResult<int>> Add(LibraryBranchDto newBranch) {
            await _context.AddAsync(newBranch);
            var newBranchId = await _context.SaveChangesAsync();
            return new ServiceResult<int> {
                Data = newBranchId,
                Error = null
            };
        }

        /// <summary>
        /// Get the business hours for the given Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<string>>> GetBranchHours(int branchId) {
            var hours = await _context
                .BranchHours
                .Where(a => a.Branch.Id == branchId)
                .ToListAsync();

            var displayHours = HumanizeBusinessHours(hours).ToList();

            return new ServiceResult<List<string>> {
                Data = displayHours,
                Error = null
            };
        }

        /// <summary>
        /// Gets a Library Branch by ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LibraryBranchDto>> Get(int branchId) {
            var branches = await _context.LibraryBranches
                .Include(b => b.Patrons)
                .Include(b => b.LibraryAssets)
                .FirstAsync(p => p.Id == branchId);

            // TODO Check if AM needs List<T>
            var branchDtos = _mapper.Map<LibraryBranchDto>(branches);
                             
            return new ServiceResult<LibraryBranchDto> {
                Data = branchDtos,
                Error = null
            };
        }

        public struct BranchHoursOpenRangeForDay {
            public int Start_SecondsSinceWeekStart { get; set; }
            public int End_SecondsSinceWeekStart { get; set; }
        }

        /// <summary>
        /// Returns true if a Library Branch is currently open.
        /// Otherwise, returns false
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResult<bool>> IsBranchOpen(int branchId) {
            var now = DateTime.UtcNow;
            
            // Get the currentSeconds since start of current week 
            // Get the branchHours
            // Create BranchHoursOpenRangeForDay for today
            // Return true if currentSeconds falls in the range the struct 
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<int>> GetAssetCount(int branchId) {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResult<int>> GetPatronCount(int branchId) {
            var branch = await Get(branchId);
            var patronCount = branch.Data.Patrons.Count();
        }

        public async Task<ServiceResult<decimal>> GetAssetsValue(int id) {
            throw new System.NotImplementedException();
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

        public int GetPatronCount(int branchId)
        {
        }

        public IEnumerable<Patron> GetPatrons(int branchId)
        {
            return _context.LibraryBranches.Include(a => a.Patrons).First(b => b.Id == branchId).Patrons;
        }
    }
}