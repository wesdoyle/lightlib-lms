using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data;
using Library.Data.Models;
using Library.Models;
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
        
        private readonly IPaginator<LibraryBranch> _branchPaginator;
        private readonly IPaginator<Patron> _patronPaginator;
        private readonly IPaginator<LibraryAsset> _assetPaginator;

        public LibraryBranchService(
            LibraryDbContext context, 
            IMapper mapper, 
            IPaginator<Patron> patronPaginator,
            IPaginator<LibraryAsset> assetPaginator,
            IPaginator<LibraryBranch> branchPaginator) {
            _context = context;
            _mapper = mapper;
            _branchPaginator = branchPaginator;
            _patronPaginator = patronPaginator;
            _assetPaginator = assetPaginator;
        }

        /// <summary>
        /// Creates a new Library Branch
        /// </summary>
        /// <param name="newBranchDto"></param>
        public async Task<ServiceResult<int>> Add(LibraryBranchDto newBranchDto) {
            var newBranch = _mapper.Map<LibraryBranch>(newBranchDto);
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
        public async Task<ServiceResult<bool>> IsBranchOpen(int branchId) {
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
        public async Task<ServiceResult<int>> GetAssetCount(int branchId) {
            var libraryBranch = await Get(branchId);
            var assetsCount = libraryBranch.Data.LibraryAssets.Count;
            return new ServiceResult<int> {
                Data = assetsCount,
                Error = null
            };
        }

        /// <summary>
        /// Returns the count of all Patrons at the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> GetPatronCount(int branchId) {
            var libraryBranch = await Get(branchId);
            var patronsCount = libraryBranch.Data.Patrons.Count;
            return new ServiceResult<int> {
                Data = patronsCount,
                Error = null
            };
        }

        /// <summary>
        /// Get the sum of the value of all Library Assets at the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<decimal>> GetAssetsValue(int branchId) {

            var branch = await _context.LibraryBranches
                .Include(a => a.LibraryAssets)
                .FirstAsync(b => b.Id == branchId);

            var assetsForBranch = branch.LibraryAssets;

            var assetsValue = assetsForBranch.Sum(a => a.Cost);
            
            return new ServiceResult<decimal> {
               Data = assetsValue,
               Error = null
            };
        }

        /// <summary>
        /// Gets a paginated collection of all Library Branches
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<LibraryBranchDto>> GetAll(int page, int perPage) {
            
            var libraryBranches = _context.LibraryBranches
                .Include(a => a.Patrons)
                .Include(a => a.LibraryAssets);

            var pageOfBranches = await _branchPaginator 
                .BuildPageResult(libraryBranches, page, perPage, ch => ch.Id)
                .ToListAsync();

            var paginatedBranches = _mapper.Map<List<LibraryBranchDto>>(pageOfBranches);
            
            var paginationResult = new PaginationResult<LibraryBranchDto> {
                Results = paginatedBranches,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<LibraryBranchDto> {
                Data = paginationResult,
                Error = null
            };
        }

        /// <summary>
        /// Get a paginated collection of Patrons for the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<PatronDto>> GetPatrons(int branchId, int page, int perPage) {
            
            var branch = await _context.LibraryBranches
                .Include(a => a.Patrons)
                .FirstAsync(b => b.Id == branchId);

            var patrons = branch.Patrons.AsQueryable();
            
            var pageOfPatrons = await _patronPaginator 
                .BuildPageResult(patrons, page, perPage, ch => ch.Id)
                .ToListAsync();

            var paginatedPatrons = _mapper.Map<List<PatronDto>>(pageOfPatrons);
            
            var paginationResult = new PaginationResult<PatronDto> {
                Results = paginatedPatrons,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<PatronDto> {
                Data = paginationResult,
                Error = null
            };
        }

        /// <summary>
        /// Get a paginated collection of Library Assets for the provided Library Branch ID
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<LibraryAssetDto>> GetAssets(int branchId, int page, int perPage) {
            
            var branch = await _context.LibraryBranches
                .Include(a => a.Patrons)
                .FirstAsync(b => b.Id == branchId);

            var libraryAssets = branch.LibraryAssets.AsQueryable();
            
            var pageOfAssets = await _assetPaginator 
                .BuildPageResult(libraryAssets, page, perPage, ch => ch.Id)
                .ToListAsync();

            var paginatedLibraryAssets = _mapper.Map<List<LibraryAssetDto>>(pageOfAssets);
            
            var paginationResult = new PaginationResult<LibraryAssetDto> {
                Results = paginatedLibraryAssets,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<LibraryAssetDto> {
                Data = paginationResult,
                Error = null
            };
        }
    }
}
