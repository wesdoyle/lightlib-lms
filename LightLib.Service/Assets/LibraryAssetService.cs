using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models.Assets;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Helpers;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Assets {
    
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

        public async Task<bool> Add(LibraryAssetDto assetDto) {
            var newAsset = _mapper.Map<LibraryAsset>(assetDto);
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

        public async Task<PaginationResult<LibraryAssetDto>> GetAll(int page, int perPage) {
            
            var assets = _context.LibraryAssets
                .Include(a => a.AvailabilityStatus)
                .Include(a => a.Location);

            var pageOfAssets = await _paginator
                .BuildPageResult(assets, page, perPage, asset => asset.Id)
                .ToListAsync();

            var pageOfAssetDtos = _mapper
                .Map<List<LibraryAssetDto>>(pageOfAssets);
            
                return new PaginationResult<LibraryAssetDto> {
                    PageNumber = page,
                    PerPage = perPage,
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
            item.AvailabilityStatus = _context.Statuses.First(a => a.Name == AssetStatus.Lost);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkFound(Guid assetId) {
            var libraryAsset = await _context.LibraryAssets
                .FirstAsync(a => a.Id == assetId);

            _context.Update(libraryAsset);
            libraryAsset.AvailabilityStatus = _context.Statuses.First(a => a.Name == AssetStatus.GoodCondition);
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
