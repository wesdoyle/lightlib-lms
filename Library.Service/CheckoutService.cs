using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace Library.Service
{
    public class CheckoutService : ICheckoutService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<Hold> _holdsPaginator;
        private readonly IPaginator<Checkout> _checkoutPaginator;

        public CheckoutService(
            LibraryDbContext context, 
            IMapper mapper, 
            IPaginator<Hold> hp, 
            IPaginator<Checkout> cp) {
            _context = context;
            _mapper = mapper;
            _holdsPaginator = hp;
            _checkoutPaginator = cp;
        }

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
        
        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistories
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);
        }

        public Task<PagedServiceResult<CheckoutDto>> GetCheckoutHistory(int id, int page, int perPage) {
            throw new NotImplementedException();
        }

        public Task<PagedServiceResult<HoldDto>> GetCurrentHolds(int id, int page, int perPage) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<CheckoutDto>> ICheckoutService.Get(int id) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<CheckoutDto>> ICheckoutService.GetLatestCheckout(int id) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<int>> ICheckoutService.GetNumberOfCopies(int id) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<bool>> ICheckoutService.IsCheckedOut(int id) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<string>> ICheckoutService.GetCurrentHoldPatron(int id) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<string>> ICheckoutService.GetCurrentHoldPlaced(int id) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<string>> ICheckoutService.GetCurrentPatron(int id) {
            throw new NotImplementedException();
        }
        
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

        Task<ServiceResult<bool>> ICheckoutService.PlaceHold(int assetId, int libraryCardId) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<bool>> ICheckoutService.CheckoutItem(int assetId, int libraryCardId) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<bool>> ICheckoutService.CheckInItem(int assetId) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<bool>> ICheckoutService.MarkLost(int assetId) {
            throw new NotImplementedException();
        }

        Task<ServiceResult<bool>> ICheckoutService.MarkFound(int assetId) {
            throw new NotImplementedException();
        }

        public Checkout Get(int id) {
            return _context.Checkouts.FirstOrDefault(p => p.Id == id);
        }

        public void CheckoutItem(int id, int libraryCardId) {
            if (IsCheckedOut(id)) return;

            var item = _context.LibraryAssets
                .Include(a => a.Status)
                .First(a => a.Id == id);

            _context.Update(item);

            item.Status = _context.Statuses
                .FirstOrDefault(a => a.Name == "Checked Out");

            var now = DateTime.Now;

            var libraryCard = _context.LibraryCards
                .Include(c => c.Checkouts)
                .FirstOrDefault(a => a.Id == libraryCardId);

            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        public void MarkLost(int id) {
            var item = _context.LibraryAssets
                .First(a => a.Id == id);

            _context.Update(item);

            item.Status = _context.Statuses.FirstOrDefault(a => a.Name == "Lost");

            _context.SaveChanges();
        }

        public void MarkFound(int id) {
            var item = _context.LibraryAssets
                .First(a => a.Id == id);

            _context.Update(item);
            item.Status = _context.Statuses.FirstOrDefault(a => a.Name == "Available");
            var now = DateTime.Now;

            // remove any existing checkouts on the item
            var checkout = _context.Checkouts
                .FirstOrDefault(a => a.LibraryAsset.Id == id);
            if (checkout != null) _context.Remove(checkout);

            // close any existing checkout history
            var history = _context.CheckoutHistories
                .FirstOrDefault(h =>
                    h.LibraryAsset.Id == id
                    && h.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }

            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCardId) {
            var now = DateTime.Now;

            var asset = _context.LibraryAssets
                .Include(a => a.Status)
                .First(a => a.Id == assetId);

            var card = _context.LibraryCards
                .First(a => a.Id == libraryCardId);

            _context.Update(asset);

            if (asset.Status.Name == "Available")
                asset.Status = _context.Statuses.FirstOrDefault(a => a.Name == "On Hold");

            var hold = new Hold {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            _context.Add(hold);
            _context.SaveChanges();
        }

        public void CheckInItem(int id) {
            var now = DateTime.Now;

            var item = _context.LibraryAssets
                .First(a => a.Id == id);

            _context.Update(item);

            // remove any existing checkouts on the item
            var checkout = _context.Checkouts
                .Include(c => c.LibraryAsset)
                .Include(c => c.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == id);
            if (checkout != null) _context.Remove(checkout);

            // close any existing checkout history
            var history = _context.CheckoutHistories
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h =>
                    h.LibraryAsset.Id == id
                    && h.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }

            // look for current holds
            var currentHolds = _context.Holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);

            // if there are current holds, check out the item to the earliest
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(id, currentHolds);
                return;
            }

            // otherwise, set item status to available
            item.Status = _context.Statuses.FirstOrDefault(a => a.Name == "Available");

            _context.SaveChanges();
        }

        // Remove useless method and replace with finding latest CheckoutHistory if needed 
        public Checkout GetLatestCheckout(int id)
        {
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == id)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();
        }

        public int GetNumberOfCopies(int id)
        {
            return _context.LibraryAssets
                .First(a => a.Id == id)
                .NumberOfCopies;
        }

        public bool IsCheckedOut(int id)
        {
            var isCheckedOut = _context.Checkouts.Any(a => a.LibraryAsset.Id == id);

            return isCheckedOut;
        }

        public string GetCurrentHoldPatron(int id)
        {
            var hold = _context.Holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(v => v.Id == id);

            var cardId = hold
                .Include(a => a.LibraryCard)
                .Select(a => a.LibraryCard.Id)
                .FirstOrDefault();

            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .First(p => p.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.LastName;
        }

        public string GetCurrentHoldPlaced(int id)
        {
            var hold = _context.Holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(v => v.Id == id);

            return hold.Select(a => a.HoldPlaced)
                .FirstOrDefault().ToString(CultureInfo.InvariantCulture);
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Where(a => a.LibraryAsset.Id == id);
        }

        public string GetCurrentPatron(int id)
        {
            var checkout = _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == id);

            if (checkout == null) return "Not checked out.";

            var cardId = checkout.LibraryCard.Id;

            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .First(c => c.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.LastName;
        }

        private void CheckoutToEarliestHold(int assetId, IEnumerable<Hold> currentHolds)
        {
            var earliestHold = currentHolds.OrderBy(a => a.HoldPlaced).FirstOrDefault();
            if (earliestHold == null) return;
            var card = earliestHold.LibraryCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();

            CheckoutItem(assetId, card.Id);
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }
    }
}