using System;
using System.Collections.Generic;
using System.Data;
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

namespace Library.Service {
    /// <summary>
    /// Handles Library Asset Checkout / Checkin / Lost / Found business logic
    /// </summary>
    public class CheckoutService : ICheckoutService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<Hold> _holdsPaginator;
        private readonly IPaginator<Checkout> _checkoutPaginator;
        private readonly IPaginator<CheckoutHistory> _checkoutHistoryPaginator;
        private readonly IHoldService _holdService;

        public CheckoutService(
            LibraryDbContext context,
            IHoldService holdService,
            IMapper mapper, 
            IPaginator<Hold> hp, 
            IPaginator<Checkout> cp,
            IPaginator<CheckoutHistory> chp
            ) {
            _context = context;
            _holdService = holdService;
            _mapper = mapper;
            _holdsPaginator = hp;
            _checkoutPaginator = cp;
            _checkoutHistoryPaginator = chp;
        }

        /// <summary>
        /// Returns a paginated result of Checkouts
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<CheckoutDto>> GetAll(int page, int perPage) {
            var checkouts = _context.Checkouts;

            var pageOfCheckouts = await _checkoutPaginator 
                .BuildPageResult(checkouts, page, perPage, b => b.Since)
                .ToListAsync();
            
            var paginatedCheckouts = _mapper.Map<List<CheckoutDto>>(pageOfCheckouts);
            
            var paginationResult = new PaginationResult<CheckoutDto> {
                Results = paginatedCheckouts,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<CheckoutDto> {
                Data = paginationResult,
                Error = null
            };
        }
        
        /// <summary>
        /// Returns an paginated Checkout History ordered by latest checked-out date
        /// </summary>
        /// <param name="libraryAssetId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<CheckoutHistoryDto>> GetCheckoutHistory(
            int libraryAssetId, 
            int page, 
            int perPage) {
            
            var checkoutHistories = _context.CheckoutHistories
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == libraryAssetId);

            var pageOfHistory = await _checkoutHistoryPaginator
                .BuildPageResult(checkoutHistories, page, perPage, ch => ch.CheckedOut)
                .ToListAsync();

            var paginatedHistories = _mapper.Map<List<CheckoutHistoryDto>>(pageOfHistory);
            
            var paginationResult = new PaginationResult<CheckoutHistoryDto> {
                Results = paginatedHistories,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<CheckoutHistoryDto> {
                Data = paginationResult,
                Error = null
            };
        }


        /// <summary>
        /// Get the Checkout corresponding to the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<CheckoutDto>> Get(int id) {
            var checkout = await _context.Checkouts
                .FirstOrDefaultAsync(p => p.Id == id);

            var checkoutDto = _mapper.Map<CheckoutDto>(checkout);
            
            return new ServiceResult<CheckoutDto> {
                Data = checkoutDto,
                Error = null
            };
        }

        /// <summary>
        /// Gets the latest Checkout for a given Library Asset ID
        /// </summary>
        /// <param name="libraryAssetId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<CheckoutDto>> GetLatestCheckout(int libraryAssetId) {
            var latest = await _context.Checkouts
                .Where(c => c.LibraryAsset.Id == libraryAssetId)
                .OrderByDescending(c => c.Since)
                .FirstOrDefaultAsync();
            
            var checkoutDto = _mapper.Map<CheckoutDto>(latest);
            
            return new ServiceResult<CheckoutDto> {
                Data = checkoutDto,
                Error = null
            };
        }

        /// <summary>
        /// Returns true if a given Library Asset ID is checked out
        /// </summary>
        /// <param name="libraryAssetId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> IsCheckedOut(int libraryAssetId) {
            var isCheckedOut = await _context.Checkouts
                .AnyAsync(a => a.LibraryAsset.Id == libraryAssetId);
            
            return new ServiceResult<bool> {
                Data = isCheckedOut,
                Error = null
            };
        }

        /// <summary>
        /// Get the patron who has the given Library Asset ID checked out
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetCurrentPatron(int id) {
            var checkout = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.LibraryAsset.Id == id);

            if (checkout == null) {
                return new ServiceResult<string> {
                    // TODO
                    Error = null,
                    Data = "Not checked out"
                };
            }

            var cardId = checkout.LibraryCard.Id;

            var patron = await _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstAsync(c => c.LibraryCard.Id == cardId);

            var patronFullName = patron.FirstName + " " + patron.LastName;
            
            return new ServiceResult<string> {
                Data = patronFullName,
                Error = null
            };
        }
        
        /// <summary>
        /// Add a checkout given a Checkout DTO representing a new instance
        /// </summary>
        /// <param name="newCheckoutDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> Add(CheckoutDto newCheckoutDto) {
            var checkoutEntity = _mapper.Map<Checkout>(newCheckoutDto);
            try {
                await _context.AddAsync(checkoutEntity);
                await _context.SaveChangesAsync();
                return new ServiceResult<int> {
                    Data = checkoutEntity.Id,
                    Error = null
                };
            } catch (Exception ex) when (
                ex is DbUpdateException 
                || ex is DBConcurrencyException) {
                
                return new ServiceResult<int> {
                    Data = 0,
                    Error = new ServiceError {
                        Message = ex.Message,
                        Stacktrace = ex.StackTrace
                    }
                };
            }
        }

        /// <summary>
        /// Checks the provided Library Asset out to the provided Library Card
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="libraryCardId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> CheckOutItem(int assetId, int libraryCardId) {

            var now = DateTime.UtcNow;

            var isAlreadyCheckedOut = await IsCheckedOut(assetId);
                
            if (isAlreadyCheckedOut.Data) {
                return new ServiceResult<bool> {
                    Data = false,
                    // TODO
                    Error = null
                };
            }

            var libraryAsset = await _context.LibraryAssets
                .Include(a => a.Status)
                .FirstAsync(a => a.Id == assetId);

            _context.Update(libraryAsset);

            // TODO
            libraryAsset.Status = await _context.Statuses
                .FirstAsync(a => a.Name == "Checked Out");

            var libraryCard = await _context.LibraryCards
                .Include(c => c.Checkouts)
                .FirstAsync(a => a.Id == libraryCardId);

            var checkout = new Checkout {
                LibraryAsset = libraryAsset,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultDateDue(now)
            };

            await _context.AddAsync(checkout);

            var checkoutHistory = new CheckoutHistory {
                CheckedOut = now,
                LibraryAsset = libraryAsset,
                LibraryCard = libraryCard
            };

            await _context.AddAsync(checkoutHistory);
            await _context.SaveChangesAsync();
            
            return new ServiceResult<bool> {
                Data = true,
                Error = null
            };
        }

        /// <summary>
        /// Checks in the given Library Asset ID
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> CheckInItem(int assetId) {
            
            var now = DateTime.UtcNow;

            var libraryAsset = await _context.LibraryAssets
                .FirstAsync(a => a.Id == assetId);

            _context.Update(libraryAsset);

            // remove any existing checkouts on the item
            var checkout = await _context.Checkouts
                .Include(c => c.LibraryAsset)
                .Include(c => c.LibraryCard)
                .FirstAsync(a => a.LibraryAsset.Id == assetId);
            
            if (checkout != null) {
                _context.Remove(checkout);
            }

            // close any existing checkout history
            var history = await _context.CheckoutHistories
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstAsync(h =>
                    h.LibraryAsset.Id == assetId 
                    && h.CheckedIn == null);
            
            if (history != null) {
                _context.Update(history);
                history.CheckedIn = now;
            }

            // if there are current holds, check out the item to the earliest
            // TODO
            var wasCheckedOutToNewHold = await CheckoutToEarliestHold(assetId);

            if (wasCheckedOutToNewHold) {
                return new ServiceResult<bool> {
                    Data = true,
                    Error = null
                };
            }

            // otherwise, set item status to available
            // TODO magic string
            libraryAsset.Status = await _context.Statuses
                .FirstAsync(a => a.Name == "Available");

            await _context.SaveChangesAsync();

            return new ServiceResult<bool> {
                Data = true,
                Error = null
            };
        }

        /// <summary>
        /// Checks the given Library Asset ID out to the next Hold
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        private async Task<bool> CheckoutToEarliestHold(int assetId) {

            var earliestHold = await _holdService.GetEarliestHold(assetId);
            
            if (earliestHold?.Data == null) {
                return false;
            }

            var card = earliestHold.Data.LibraryCard;
            
            _context.Remove(earliestHold);
            await _context.SaveChangesAsync();
            
            // TODO
            var checkOutResult = await CheckOutItem(assetId, card.Id);
            
            return checkOutResult.Data;
        }

        /// <summary>
        /// Gets default date an asset is due
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        /// TODO Magic Number
        private static DateTime GetDefaultDateDue(DateTime now) => now.AddDays(30);
    }
}