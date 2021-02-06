using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface ILibraryAssetService {
        Task<PaginationResult<LibraryAssetDto>> GetAll(int page, int perPage);
        Task<LibraryAssetDto> Get(int id);
        Task<bool> Add(LibraryAssetDto newDto);
        Task<string> GetAuthorOrDirector(int assetId);
        Task<LibraryBranchDto> GetCurrentLocation(int id);
        Task<int> GetNumberOfCopies(int libraryAssetId);
        Task<bool> MarkLost(int assetId);
        Task<bool> MarkFound(int assetId);
    }
}