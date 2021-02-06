using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Helpers;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service {
    /// <summary>
    /// Handles business logic related to Library Assets 
    /// </summary>
    public class LibraryAssetService : ILibraryAssetService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Paginator<LibraryAsset> _paginator;

        public LibraryAssetService(
            LibraryDbContext context, 
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
            _paginator = new Paginator<LibraryAsset>();
        }

        /// <summary>
        /// Creates a new Library Asset
        /// </summary>
        /// <param name="assetDto"></param>
        /// <returns></returns>
        public async Task<bool> Add(LibraryAssetDto assetDto) {
            var newAsset = _mapper.Map<LibraryAsset>(assetDto);
            await _context.AddAsync(newAsset);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets a Library Asset by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LibraryAssetDto> Get(int id) {
            var asset = await _context.LibraryAssets
                .Include(a => a.Status)
                .Include(a => a.Location)
                .FirstAsync(a => a.Id == id);
            return _mapper.Map<LibraryAssetDto>(asset);
        }

        /// <summary>
        /// Get a paginated list of Library Assets
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<LibraryAssetDto>> GetAll(int page, int perPage) {
            
            var assets = _context.LibraryAssets
                .Include(a => a.Status)
                .Include(a => a.Location);

            var pageOfAssets = await _paginator
                .BuildPageResult(assets, page, perPage, a => a.Title)
                .ToListAsync();

            var pageOfAssetDtos = _mapper
                .Map<List<LibraryAssetDto>>(pageOfAssets);
            
                return new PaginationResult<LibraryAssetDto> {
                    PageNumber = page,
                    PerPage = perPage,
                    Results = pageOfAssetDtos 
            };
        }

        /// <summary>
        /// Gets the Author or Director for a given assetId
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public async Task<string> GetAuthorOrDirector(int assetId) {
            var isBook = await _context.LibraryAssets
                .OfType<Book>()
                .AnyAsync(a => a.Id == assetId);

            return isBook
                ? await GetAuthor(assetId)
                : await GetDirector(assetId);
        }

        /// <summary>
        /// Gets the Director of a Video Library Asset
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<string> GetDirector(int assetId) {
            // TODO
            return "";
        }

        /// <summary>
        /// Gets the Author of Book Library Asset
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        private async Task<string> GetAuthor(int assetId) {
            // TODO
            return "";
        }

        /// <summary>
        /// Gets the Current Library Branch of the given Library Asset ID
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public async Task<LibraryBranchDto> GetCurrentLocation(int assetId) {
            var asset = await _context
                .LibraryAssets
                .FirstAsync(a => a.Id == assetId);
            var location = asset.Location;
            return _mapper.Map<LibraryBranchDto>(location);
        }
        
        /// <summary>
        /// Marks the given Library Asset ID as Lost
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public async Task<bool> MarkLost(int assetId) {
            var item = await _context.LibraryAssets
                .FirstAsync(a => a.Id == assetId);
            _context.Update(item);
            // TODO
            item.Status = _context.Statuses.First(a => a.Name == "Lost");
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Marks the given Library Asset ID as Found
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public async Task<bool> MarkFound(int assetId) {
            var libraryAsset = await _context.LibraryAssets
                .FirstAsync(a => a.Id == assetId);

            _context.Update(libraryAsset);
            libraryAsset.Status = _context.Statuses.First(a => a.Name == "Available");
            var now = DateTime.UtcNow;

            // remove any existing checkouts on the item
            var checkout = _context.Checkouts
                .First(a => a.LibraryAsset.Id == assetId);
            if (checkout != null) _context.Remove(checkout);

            // close any existing checkout history
            var history = _context.CheckoutHistories
                .First(h =>
                    h.LibraryAsset.Id == assetId 
                    && h.CheckedIn == null);
            
            if (history != null) {
                _context.Update(history);
                history.CheckedIn = now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
