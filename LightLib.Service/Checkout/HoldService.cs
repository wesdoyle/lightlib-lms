using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Checkout {
    
    public class HoldService : IHoldService {
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public HoldService(
            LibraryDbContext context,
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginationResult<HoldDto>> GetCurrentHoldsPaginated(Guid assetId, int page, int perPage) {
            
            var holds = _context.Holds
                .Include(h => h.Asset)
                .Where(a => a.Asset.Id == assetId);
            var pageOfHolds = await holds.ToPaginatedResult(page, perPage);
            var pageOfAssetDtos = _mapper.Map<List<HoldDto>>(pageOfHolds.Results);
            return new PaginationResult<HoldDto> {
                    PageNumber = pageOfHolds.PageNumber,
                    PerPage = pageOfHolds.PerPage,
                    Results = pageOfAssetDtos 
            };
        }
        
        public async Task<string> GetCurrentHoldPatron(int holdId) {
            var hold = _context.Holds
                .Include(a => a.Asset)
                .Include(a => a.LibraryCard)
                .Where(v => v.Id == holdId);

            var cardId = await hold
                .Include(a => a.LibraryCard)
                .Select(a => a.LibraryCard.Id)
                .FirstAsync();

            var patron = await _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstAsync(p => p.LibraryCard.Id == cardId);

            return $"{patron.FirstName} {patron.LastName}";
        }

        public async Task<string> GetCurrentHoldPlaced(int holdId) {
            var hold = await _context.Holds
                .Include(a => a.Asset)
                .Include(a => a.LibraryCard)
                .FirstAsync(v => v.Id == holdId);

            var holdPlaced = hold.HoldPlaced;

            return holdPlaced.ToString(CultureInfo.InvariantCulture);
        }

        public async Task<bool> PlaceHold(Guid assetId, int libraryCardId) {
            var now = DateTime.UtcNow;

            var asset = await _context.LibraryAssets
                .Include(a => a.AvailabilityStatus)
                .FirstAsync(a => a.Id == assetId);

            var card = await _context.LibraryCards
                .FirstAsync(a => a.Id == libraryCardId);

            _context.Update(asset);

            if (asset.AvailabilityStatus.Name == "Available") {
                asset.AvailabilityStatus = await _context.Statuses
                    .FirstAsync(a => a.Name == "On Hold");
            }

            var hold = new Hold {
                HoldPlaced = now,
                Asset = asset,
                LibraryCard = card
            };

            await _context.AddAsync(hold);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<HoldDto> GetEarliestHold(Guid assetId) {
            var earliestHold = await _context.Holds
                .Include(hold => hold.Asset)
                .Include(hold => hold.LibraryCard)
                .Where(hold => hold.Asset.Id == assetId)
                .OrderBy(a => a.HoldPlaced)
                .FirstAsync();

            return _mapper.Map<HoldDto>(earliestHold);
        }
    }
}
