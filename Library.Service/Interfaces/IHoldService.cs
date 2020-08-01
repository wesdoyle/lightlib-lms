using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface IHoldService {
        Task<PagedServiceResult<HoldDto>> GetCurrentHolds(int id, int page, int perPage);
        Task<ServiceResult<bool>> PlaceHold(int assetId, int libraryCardId);
        Task<ServiceResult<string>> GetCurrentHoldPatron(int holdId);
        Task<ServiceResult<string>> GetCurrentHoldPlaced(int holdId);
        Task<ServiceResult<HoldDto>> GetEarliestHold(int libraryAssetId);
    }
}