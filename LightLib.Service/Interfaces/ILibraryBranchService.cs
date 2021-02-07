using System.Collections.Generic;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Service.Interfaces {
    public interface ILibraryBranchService {
        Task<PaginationResult<LibraryBranchDto>> GetPaginated(int page, int perPage);
        Task<PaginationResult<PatronDto>> GetPatrons(int branchId, int page, int perPage);
        Task<PaginationResult<LibraryAssetDto>> GetAssets(int branchId, int page, int perPage);
        Task<List<string>> GetBranchHours(int branchId);
        Task<LibraryBranchDto> Get(int branchId);
        Task<bool> Add(LibraryBranchDto newBranchDto);
        Task<bool> IsBranchOpen(int branchId);
        Task<int> GetAssetCount(int branchId);
        Task<int> GetPatronCount(int branchId);
        Task<decimal> GetAssetsValue(int branchId);
    }
}