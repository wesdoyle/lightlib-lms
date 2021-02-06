using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface IHoldService {
        Task<PaginationResult<HoldDto>> GetCurrentHolds(int id, int page, int perPage);
        Task<bool> PlaceHold(int assetId, int libraryCardId);
        Task<string> GetCurrentHoldPatron(int holdId);
        Task<string> GetCurrentHoldPlaced(int holdId);
        Task<HoldDto> GetEarliestHold(int libraryAssetId);
    }
}