using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.DTOs.Assets;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using static LightLib.Service.Helpers.DataHelpers;

namespace LightLib.Service.Branches {
    
    public class LibraryBranchService : ILibraryBranchService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        public LibraryBranchService(LibraryDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<bool> Add(LibraryBranchDto newBranchDto) {
            var newBranch = _mapper.Map<LibraryBranch>(newBranchDto);
            await _context.AddAsync(newBranch);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetBranchHours(int branchId) {
            var hours = await _context
                .BranchHours
                .Where(a => a.Branch.Id == branchId)
                .ToListAsync();
            return HumanizeBusinessHours(hours).ToList();
        }

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

        public async Task<bool> IsBranchOpen(int branchId) {
            var now = DateTime.UtcNow;
            
            // TODO
            // Get the currentSeconds since start of current week 
            // Get the branchHours
            // Create BranchHoursOpenRangeForDay for today
            // Return true if currentSeconds falls in the range the struct 
            return true;
        }

        public async Task<int> GetAssetCount(int branchId) {
            var libraryBranch = await Get(branchId);
            return libraryBranch.LibraryAssets.Count;
        }

        public async Task<int> GetPatronCount(int branchId) {
            var libraryBranch = await Get(branchId);
            return libraryBranch.Patrons.Count;
        }

        public async Task<decimal> GetAssetsValue(int branchId) {
            var branch = await _context.LibraryBranches
                .Include(a => a.LibraryAssets)
                .FirstAsync(b => b.Id == branchId);
            var assetsForBranch = branch.LibraryAssets;
            return assetsForBranch.Sum(a => a.Cost);
        }

        public async Task<PaginationResult<LibraryBranchDto>> GetPaginated(int page, int perPage) {
            var libraryBranches = _context.LibraryBranches
                .Include(a => a.Patrons)
                .Include(a => a.LibraryAssets);
            var pageOfBranches = await libraryBranches.ToPaginatedResult(page, perPage);
            var pageOfAssetDtos = _mapper.Map<List<LibraryBranchDto>>(pageOfBranches.Results);
            return new PaginationResult<LibraryBranchDto> {
                    PageNumber = pageOfBranches.PageNumber,
                    PerPage = pageOfBranches.PerPage,
                    Results = pageOfAssetDtos 
            };
        }

        public async Task<PaginationResult<PatronDto>> GetPatrons(int branchId, int page, int perPage) {
            var branch = await _context.LibraryBranches
                .Include(a => a.Patrons)
                .FirstAsync(b => b.Id == branchId);
            var patrons = branch.Patrons.AsQueryable();
            var pageOfPatrons = await patrons.ToPaginatedResult(page, perPage);
            var pageOfAssetDtos = _mapper.Map<List<PatronDto>>(pageOfPatrons.Results);
            return new PaginationResult<PatronDto> {
                    PageNumber = pageOfPatrons.PageNumber,
                    PerPage = pageOfPatrons.PerPage,
                    Results = pageOfAssetDtos 
            };
        }

        public async Task<PaginationResult<LibraryAssetDto>> GetAssets(int branchId, int page, int perPage) {
            var branch = await _context.LibraryBranches
                .Include(a => a.Patrons)
                .FirstAsync(b => b.Id == branchId);
            var libraryAssets = branch.LibraryAssets.AsQueryable();
            var pageOfAssets = await libraryAssets.ToPaginatedResult(page, perPage);
            var pageOfAssetDtos = _mapper.Map<List<LibraryAssetDto>>(pageOfAssets.Results);
            return new PaginationResult<LibraryAssetDto> {
                    PageNumber = pageOfAssets.PageNumber,
                    PerPage = pageOfAssets.PerPage,
                    Results = pageOfAssetDtos 
            };
        }
    }
}
