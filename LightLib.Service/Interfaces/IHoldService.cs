using System;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface IHoldService {
        Task<PaginationResult<HoldDto>> GetCurrentHoldsPaginated(Guid assetId, int page, int perPage);
        Task<bool> PlaceHold(Guid assetId, int libraryCardId);
        Task<string> GetCurrentHoldPatron(int holdId);
        Task<string> GetCurrentHoldPlaced(int holdId);
        Task<HoldDto> GetEarliestHold(Guid assetId);
    }
}