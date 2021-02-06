using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface ICheckoutService {

        Task<bool> Add(CheckoutDto newCheckout);
        Task<CheckoutDto> Get(int id);
        Task<bool> CheckInItem(int assetId);
        Task<bool> CheckOutItem(int assetId, int libraryCardId);
        Task<CheckoutDto> GetLatestCheckout(int id);
        Task<bool> IsCheckedOut(int libraryAssetId);
        Task<string> GetCurrentPatron(int id);
        
        Task<PaginationResult<CheckoutDto>> GetAll(int page, int perPage);
        Task<PaginationResult<CheckoutHistoryDto>> GetCheckoutHistory(int id, int page, int perPage);
    }
}