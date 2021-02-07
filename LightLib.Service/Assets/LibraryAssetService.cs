using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models.Assets;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.DTOs.Assets;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Assets {
    
    public class LibraryAssetService : ILibraryAssetService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public LibraryAssetService(
            LibraryDbContext context, 
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Add(LibraryAssetDto assetDto) {
            var newAsset = _mapper.Map<Asset>(assetDto);
            await _context.AddAsync(newAsset);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LibraryAssetDto> Get(Guid assetId) {
            var asset = await _context.LibraryAssets
                .Include(a => a.AvailabilityStatus)
                .Include(a => a.Location)
                .FirstAsync(a => a.Id == assetId);
            return _mapper.Map<LibraryAssetDto>(asset);
        }

        public async Task<PaginationResult<LibraryAssetDto>> GetPaginated(int page, int perPage) {
            var assets = _context.LibraryAssets
                .Include(a => a.AvailabilityStatus)
                .Include(a => a.Location);
            var pageOfAssets = await assets.ToPaginatedResult(page, perPage);
            var pageOfAssetDtos = _mapper.Map<List<LibraryAssetDto>>(pageOfAssets.Results);
            return new PaginationResult<LibraryAssetDto> {
                    PageNumber = pageOfAssets.PageNumber,
                    PerPage = pageOfAssets.PerPage,
                    Results = pageOfAssetDtos 
            };
        }

        public async Task<LibraryBranchDto> GetCurrentLocation(Guid assetId) {
            var asset = await _context
                .LibraryAssets
                .FirstAsync(a => a.Id == assetId);
            var location = asset.Location;
            return _mapper.Map<LibraryBranchDto>(location);
        }
        
        public async Task<bool> MarkLost(Guid assetId) {
            var item = await _context.LibraryAssets
                .FirstAsync(a => a.Id == assetId);
            _context.Update(item);
            // TODO
            item.AvailabilityStatus = _context.Statuses
                .First(a => a.Name == AssetStatus.Lost);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkFound(Guid assetId) {
            var libraryAsset = await _context.LibraryAssets
                .FirstAsync(a => a.Id == assetId);
            _context.Update(libraryAsset);
            libraryAsset.AvailabilityStatus = _context.Statuses
                .First(a => a.Name == AssetStatus.GoodCondition);
            var now = DateTime.UtcNow;
            
            // remove any existing checkouts on the item
            var checkout = _context.Checkouts
                .First(a => a.Asset.Id == assetId);
            if (checkout != null) _context.Remove(checkout);

            // close any existing checkout history
            var history = _context.CheckoutHistories
                .First(h =>
                    h.Asset.Id == assetId 
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
