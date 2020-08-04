using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface ILibraryCardService {
        Task<PagedServiceResult<LibraryCardDto>> GetAll(int page, int perPage);
        Task<ServiceResult<LibraryCardDto>> Get(int libraryCardId);
        Task<ServiceResult<int>> Add(LibraryCardDto libraryCardDto);
    }
}