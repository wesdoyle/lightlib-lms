using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.Exceptions;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Checkout {
    
    public class CheckoutService : ICheckoutService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHoldService _holdService;
        
        private const int DefaultDateDueDays = 30;

        public CheckoutService(
            LibraryDbContext context,
            IHoldService holdService,
            IMapper mapper) {
            _context = context;
            _holdService = holdService;
            _mapper = mapper;
        }

        public async Task<PaginationResult<CheckoutDto>> GetPaginated(int page, int perPage) {
            var checkouts = _context.Checkouts;
            var pageOfCheckouts = await checkouts.ToPaginatedResult(page, perPage);
            var pageOfAssetDtos = _mapper.Map<List<CheckoutDto>>(pageOfCheckouts.Results);
            return new PaginationResult<CheckoutDto> {
                    PageNumber = pageOfCheckouts.PageNumber,
                    PerPage = pageOfCheckouts.PerPage,
                    Results = pageOfAssetDtos 
            };
        }

        public async Task<PaginationResult<CheckoutHistoryDto>> GetCheckoutHistory(
            Guid assetId, 
            int page, 
            int perPage) {
            
            var checkoutHistories = _context.CheckoutHistories
                .Include(a => a.Asset)
                .Include(a => a.LibraryCard)
                .Where(a => a.Asset.Id == assetId);

            var pageOfHistory = await checkoutHistories.ToPaginatedResult(page, perPage);
            var pageOfHistoryDto = _mapper.Map<List<CheckoutHistoryDto>>(pageOfHistory.Results);
            return new PaginationResult<CheckoutHistoryDto> {
                    PageNumber = pageOfHistory.PageNumber,
                    PerPage = pageOfHistory.PerPage,
                    Results = pageOfHistoryDto 
            };
        }

        public async Task<CheckoutDto> Get(int checkoutId) {
            var checkout = await _context.Checkouts.FirstAsync(p => p.Id == checkoutId);
            return _mapper.Map<CheckoutDto>(checkout);
        }

        public async Task<CheckoutDto> GetLatestCheckout(Guid assetId) {
            var latest = await _context.Checkouts
                .Where(c => c.Asset.Id == assetId)
                .OrderByDescending(c => c.CheckedOutSince)
                .FirstAsync();
            return _mapper.Map<CheckoutDto>(latest);
        }

        public async Task<bool> IsCheckedOut(Guid assetId) 
            => await _context.Checkouts .AnyAsync(a => a.Asset.Id == assetId);

        public async Task<string> GetCurrentPatron(Guid assetId) {
            var checkout = await _context.Checkouts
                .Include(a => a.Asset)
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Asset.Id == assetId);

            if (checkout == null) {
                // TODO
            }

            var cardId = checkout.LibraryCard.Id;

            var patron = await _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstAsync(c => c.LibraryCard.Id == cardId);

            return $"{patron.FirstName} {patron.LastName}";
        }
        
        public async Task<bool> Add(CheckoutDto newCheckoutDto) {
            var checkoutEntity = _mapper.Map<Data.Models.Checkout>(newCheckoutDto);
            try {
                await _context.AddAsync(checkoutEntity);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex) {
                throw new LibraryServiceException(Reason.UncaughtError);
            }
        }

        public async Task<bool> CheckOutItem(Guid assetId, int libraryCardId) {

            var now = DateTime.UtcNow;

            var isAlreadyCheckedOut = await IsCheckedOut(assetId);
                
            if (isAlreadyCheckedOut) {
                // TODO
            }

            var libraryAsset = await _context.LibraryAssets
                .Include(a => a.AvailabilityStatus)
                .FirstAsync(a => a.Id == assetId);

            _context.Update(libraryAsset);

            // TODO
            libraryAsset.AvailabilityStatus = await _context.Statuses
                .FirstAsync(a => a.Name == "Checked Out");

            var libraryCard = await _context.LibraryCards
                .Include(c => c.Checkouts)
                .FirstAsync(a => a.Id == libraryCardId);

            var checkout = new Data.Models.Checkout {
                Asset = libraryAsset,
                LibraryCard = libraryCard,
                CheckedOutSince = now,
                CheckedOutUntil = GetDefaultDateDue(now)
            };

            await _context.AddAsync(checkout);

            var checkoutHistory = new CheckoutHistory {
                CheckedOut = now,
                Asset = libraryAsset,
                LibraryCard = libraryCard
            };

            await _context.AddAsync(checkoutHistory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckInItem(Guid assetId) {
            
            var now = DateTime.UtcNow;

            var libraryAsset = await _context.LibraryAssets
                .FirstAsync(a => a.Id == assetId);

            _context.Update(libraryAsset);

            // remove any existing checkouts on the item
            var checkout = await _context.Checkouts
                .Include(c => c.Asset)
                .Include(c => c.LibraryCard)
                .FirstAsync(a => a.Asset.Id == assetId);
            
            if (checkout != null) {
                _context.Remove(checkout);
            }

            // close any existing checkout history
            var history = await _context.CheckoutHistories
                .Include(h => h.Asset)
                .Include(h => h.LibraryCard)
                .FirstAsync(h =>
                    h.Asset.Id == assetId 
                    && h.CheckedIn == null);
            
            if (history != null) {
                _context.Update(history);
                history.CheckedIn = now;
            }

            // if there are current holds, check out the item to the earliest
            // TODO
            var wasCheckedOutToNewHold = await CheckoutToEarliestHold(assetId);

            if (wasCheckedOutToNewHold) {
                // TODO
            }

            // otherwise, set item status to available
            // TODO magic string
            libraryAsset.AvailabilityStatus = await _context.Statuses
                .FirstAsync(a => a.Name == "Available");

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> CheckoutToEarliestHold(Guid assetId) {

            var earliestHold = await _holdService.GetEarliestHold(assetId);
            
            if (earliestHold == null) {
                return false;
            }

            var card = earliestHold.LibraryCard;
            
            _context.Remove(earliestHold);
            await _context.SaveChangesAsync();
            
            // TODO
            var checkOutResult = await CheckOutItem(assetId, card.Id);
            
            return checkOutResult;
        }

        private static DateTime GetDefaultDateDue(DateTime now) => now.AddDays(DefaultDateDueDays);
    }
}
