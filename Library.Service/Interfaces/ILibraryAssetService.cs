using System.Threading.Tasks;
using Library.Data.Models;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface ILibraryAssetService {
        Task<PagedServiceResult<LibraryAsset>> GetAll(int page, int perPage);
        Task<ServiceResult<LibraryAsset>> Get(int id);
        Task<ServiceResult<int>> Add(AssetType newType);
        
        Task<ServiceResult<string>> GetAuthorOrDirector(int id);
        Task<ServiceResult<string>> GetDeweyIndex(int id);
        Task<ServiceResult<string>> GetType(int id);
        Task<ServiceResult<string>> GetTitle(int id);
        Task<ServiceResult<string>> GetIsbn(int id);
        
        Task<ServiceResult<LibraryBranch>> GetCurrentLocation(int id);
        Task<ServiceResult<LibraryCard>> GetLibraryCardByAssetId(int id);
    }
}