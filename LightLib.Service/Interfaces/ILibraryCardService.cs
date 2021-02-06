using System.Threading.Tasks;
using LightLib.Models.DTOs;
using LightLib.Service.Models;

namespace LightLib.Service.Interfaces {
    public interface ILibraryCardService {
        Task<PagedServiceResult<LibraryCardDto>> GetAll(int page, int perPage);
        Task<ServiceResult<LibraryCardDto>> Get(int libraryCardId);
        Task<ServiceResult<int>> Add(LibraryCardDto libraryCardDto);
    }
}