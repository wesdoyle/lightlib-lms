using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface ILibraryCardService {
        Task<PaginationResult<LibraryCardDto>> GetPaginated(int page, int perPage);
        Task<LibraryCardDto> Get(int libraryCardId);
        Task<bool> Add(LibraryCardDto libraryCardDto);
    }
}