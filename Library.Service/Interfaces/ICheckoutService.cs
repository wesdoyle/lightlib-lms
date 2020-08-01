using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface ICheckoutService {

        Task<ServiceResult<int>> Add(CheckoutDto newCheckout);
        Task<ServiceResult<CheckoutDto>> Get(int id);
        Task<ServiceResult<bool>> CheckInItem(int assetId);
        Task<ServiceResult<bool>> CheckOutItem(int assetId, int libraryCardId);
        Task<ServiceResult<CheckoutDto>> GetLatestCheckout(int id);
        Task<ServiceResult<bool>> IsCheckedOut(int libraryAssetId);
        Task<ServiceResult<string>> GetCurrentPatron(int id);
        
        Task<PagedServiceResult<CheckoutDto>> GetAll(int page, int perPage);
        Task<PagedServiceResult<CheckoutHistoryDto>> GetCheckoutHistory(int id, int page, int perPage);
    }
}