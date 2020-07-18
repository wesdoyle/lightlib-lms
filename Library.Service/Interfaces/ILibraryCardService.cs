using System.Threading.Tasks;
using Library.Data.Models;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface ILibraryCardService {
        Task<PagedServiceResult<LibraryCard>> GetAll(int page, int perPage);
        Task<ServiceResult<LibraryCard>> Get(int libraryCardId);
        Task<ServiceResult<int>> Add(LibraryCardDto libraryCardDto);
    }
}