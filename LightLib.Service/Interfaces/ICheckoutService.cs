using System;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface ICheckoutService {
        Task<bool> Add(CheckoutDto newCheckout);
        Task<CheckoutDto> Get(int checkoutId);
        Task<bool> CheckInItem(Guid assetId);
        Task<bool> CheckOutItem(Guid assetId, int libraryCardId);
        Task<CheckoutDto> GetLatestCheckout(Guid assetId);
        Task<bool> IsCheckedOut(Guid assetId);
        Task<string> GetCurrentPatron(Guid assetId);
        Task<PaginationResult<CheckoutDto>> GetPaginated(int page, int perPage);
        Task<PaginationResult<CheckoutHistoryDto>> GetCheckoutHistory(Guid assetId, int page, int perPage);
    }
}