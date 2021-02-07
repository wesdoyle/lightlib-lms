using System;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Service.Interfaces {
    public interface ILibraryAssetService {
        Task<PaginationResult<LibraryAssetDto>> GetPaginated(int page, int perPage);
        Task<LibraryAssetDto> Get(Guid assetId);
        Task<bool> Add(LibraryAssetDto newDto);
        Task<LibraryBranchDto> GetCurrentLocation(Guid assetId);
        Task<bool> MarkLost(Guid assetId);
        Task<bool> MarkFound(Guid assetId);
    }
}