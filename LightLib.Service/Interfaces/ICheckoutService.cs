using System.Threading.Tasks;
using LightLib.Models.DTOs;
using LightLib.Service.Models;

namespace LightLib.Service.Interfaces {
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