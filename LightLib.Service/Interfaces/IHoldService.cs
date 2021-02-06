using System.Threading.Tasks;
using LightLib.Models.DTOs;
using LightLib.Service.Models;

namespace LightLib.Service.Interfaces {
    public interface IHoldService {
        Task<PagedServiceResult<HoldDto>> GetCurrentHolds(int id, int page, int perPage);
        Task<ServiceResult<bool>> PlaceHold(int assetId, int libraryCardId);
        Task<ServiceResult<string>> GetCurrentHoldPatron(int holdId);
        Task<ServiceResult<string>> GetCurrentHoldPlaced(int holdId);
        Task<ServiceResult<HoldDto>> GetEarliestHold(int libraryAssetId);
    }
}