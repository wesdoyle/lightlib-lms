using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface ILibraryAssetService {
        Task<PagedServiceResult<LibraryAssetDto>> GetAll(int page, int perPage);
        Task<ServiceResult<LibraryAssetDto>> Get(int id);
        Task<ServiceResult<int>> Add(AssetTypeDto newType);
        
        Task<ServiceResult<string>> GetAuthorOrDirector(int id);
        Task<ServiceResult<string>> GetDeweyIndex(int id);
        Task<ServiceResult<string>> GetType(int id);
        Task<ServiceResult<string>> GetTitle(int id);
        Task<ServiceResult<string>> GetIsbn(int id);
        
        Task<ServiceResult<LibraryBranchDto>> GetCurrentLocation(int id);
        Task<ServiceResult<LibraryCardDto>> GetLibraryCardByAssetId(int id);
    }
}