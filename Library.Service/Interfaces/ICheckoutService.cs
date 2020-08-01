using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface ICheckoutService {
        Task<PagedServiceResult<CheckoutDto>> GetAll(int page, int perPage);
        Task<PagedServiceResult<CheckoutHistoryDto>> GetCheckoutHistory(int id, int page, int perPage);
        Task<PagedServiceResult<HoldDto>> GetCurrentHolds(int id, int page, int perPage);

        Task<ServiceResult<CheckoutDto>> Get(int id);
        Task<ServiceResult<CheckoutDto>> GetLatestCheckout(int id);
        
        Task<ServiceResult<int>> GetNumberOfCopies(int id);
        Task<ServiceResult<bool>> IsCheckedOut(int id);
        
        Task<ServiceResult<string>> GetCurrentHoldPatron(int id);
        Task<ServiceResult<string>> GetCurrentHoldPlaced(int id);
        Task<ServiceResult<string>> GetCurrentPatron(int id);
        
        Task<ServiceResult<int>> Add(CheckoutDto newCheckout);
        
        Task<ServiceResult<bool>> PlaceHold(int assetId, int libraryCardId);
        Task<ServiceResult<bool>> CheckoutItem(int assetId, int libraryCardId);
        Task<ServiceResult<bool>> CheckInItem(int assetId);
        
        Task<ServiceResult<bool>> MarkLost(int assetId);
        Task<ServiceResult<bool>> MarkFound(int assetId);
    }
}