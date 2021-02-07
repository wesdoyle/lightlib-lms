using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Data.Models.Assets;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Helpers;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using static LightLib.Service.Helpers.DataHelpers;

namespace LightLib.Service.Branches {
    /// <summary>
    /// Handles business logic related to Library Branches 
    /// </summary>
    public class LibraryBranchService : ILibraryBranchService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Paginator<LibraryBranch> _branchPaginator;
        private readonly Paginator<Patron> _patronPaginator;
        private readonly Paginator<Asset> _assetPaginator;
        public LibraryBranchService(LibraryDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
            _branchPaginator = new Paginator<LibraryBranch>();
            _patronPaginator = new Paginator<Patron>();
            _assetPaginator = new Paginator<Asset>();
        }

        /// <summary>
        /// Creates a new Library Branch
        /// </summary>
        /// <param name="newBranchDto"></param>
        public async Task<bool> Add(LibraryBranchDto newBranchDto) {
            var newBranch = _mapper.Map<LibraryBranch>(newBranchDto);
            await _context.AddAsync(newBranch);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Get the business hours for the given Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBranchHours(int branchId) {
            var hours = await _context
                .BranchHours
                .Where(a => a.Branch.Id == branchId)
                .ToListAsync();
            return HumanizeBusinessHours(hours).ToList();
        }

        /// <summary>
        /// Gets a Library Branch by ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<LibraryBranchDto> Get(int branchId) {
            var branches = await _context.LibraryBranches
                .Include(b => b.Patrons)
                .Include(b => b.LibraryAssets)
                .FirstAsync(p => p.Id == branchId);

            // TODO Check if AM needs List<T>
            return _mapper.Map<LibraryBranchDto>(branches);
        }

        // TODO
        private struct BranchHoursOpenRangeForDay {
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
        public async Task<bool> IsBranchOpen(int branchId) {
            var now = DateTime.UtcNow;
            
            // Get the currentSeconds since start of current week 
            // Get the branchHours
            // Create BranchHoursOpenRangeForDay for today
            // Return true if currentSeconds falls in the range the struct 
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the count of all LibraryAssets at the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<int> GetAssetCount(int branchId) {
            var libraryBranch = await Get(branchId);
            return libraryBranch.LibraryAssets.Count;
        }

        /// <summary>
        /// Returns the count of all Patrons at the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<int> GetPatronCount(int branchId) {
            var libraryBranch = await Get(branchId);
            return libraryBranch.Patrons.Count;
        }

        /// <summary>
        /// Get the sum of the value of all Library Assets at the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<decimal> GetAssetsValue(int branchId) {
            var branch = await _context.LibraryBranches
                .Include(a => a.LibraryAssets)
                .FirstAsync(b => b.Id == branchId);
            var assetsForBranch = branch.LibraryAssets;
            return assetsForBranch.Sum(a => a.Cost);
        }

        /// <summary>
        /// Gets a paginated collection of all Library Branches
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<LibraryBranchDto>> GetAll(int page, int perPage) {
            
            var libraryBranches = _context.LibraryBranches
                .Include(a => a.Patrons)
                .Include(a => a.LibraryAssets);

            var pageOfBranches = await _branchPaginator 
                .BuildPageResult(libraryBranches, page, perPage, ch => ch.Id)
                .ToListAsync();

            var paginatedBranches = _mapper.Map<List<LibraryBranchDto>>(pageOfBranches);
            
            return new PaginationResult<LibraryBranchDto> {
                Results = paginatedBranches,
                PerPage = perPage,
                PageNumber = page
            };
        }

        /// <summary>
        /// Get a paginated collection of Patrons for the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<PatronDto>> GetPatrons(int branchId, int page, int perPage) {
            
            var branch = await _context.LibraryBranches
                .Include(a => a.Patrons)
                .FirstAsync(b => b.Id == branchId);

            var patrons = branch.Patrons.AsQueryable();
            
            var pageOfPatrons = await _patronPaginator 
                .BuildPageResult(patrons, page, perPage, ch => ch.Id)
                .ToListAsync();

            var paginatedPatrons = _mapper.Map<List<PatronDto>>(pageOfPatrons);
            
            return new PaginationResult<PatronDto> {
                Results = paginatedPatrons,
                PerPage = perPage,
                PageNumber = page
            };
        }

        /// <summary>
        /// Get a paginated collection of Library Assets for the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<LibraryAssetDto>> GetAssets(int branchId, int page, int perPage) {
            
            var branch = await _context.LibraryBranches
                .Include(a => a.Patrons)
                .FirstAsync(b => b.Id == branchId);

            var libraryAssets = branch.LibraryAssets.AsQueryable();
            
            var pageOfAssets = await _assetPaginator 
                .BuildPageResult(libraryAssets, page, perPage, ch => ch.Id)
                .ToListAsync();

            var paginatedLibraryAssets = _mapper.Map<List<LibraryAssetDto>>(pageOfAssets);
            
            return new PaginationResult<LibraryAssetDto> {
                Results = paginatedLibraryAssets,
                PerPage = perPage,
                PageNumber = page
            };
        }
    }
}
