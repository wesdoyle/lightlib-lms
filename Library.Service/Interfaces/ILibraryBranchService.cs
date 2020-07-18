using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface ILibraryBranchService {
        Task<PagedServiceResult<LibraryBranchDto>> GetAll(int page, int perPage);
        Task<PagedServiceResult<PatronDto>> GetPatrons(int page, int perPage);
        Task<PagedServiceResult<LibraryAssetDto>> GetAssets(int page, int perPage);

        Task<ServiceResult<List<string>>> GetBranchHours(int branchId);
        Task<ServiceResult<LibraryBranchDto>> Get(int branchId);
        
        Task<ServiceResult<int>> Add(LibraryBranchDto newBranch);
        
        Task<ServiceResult<bool>> IsBranchOpen(int branchId);
        Task<ServiceResult<int>> GetAssetCount(int branchId);
        Task<ServiceResult<int>> GetPatronCount(int branchId);
        Task<ServiceResult<decimal>> GetAssetsValue(int id);
    }
}