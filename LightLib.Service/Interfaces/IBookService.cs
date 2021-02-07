using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Service.Interfaces {
    public interface IBookService {
        Task<PaginationResult<BookDto>> GetAll(int page, int pageNumber);
        Task<PaginationResult<BookDto>> GetByAuthor(string author, int page, int perPage);
        Task<BookDto> GetByIsbn(string isbn);
        Task<BookDto> Get(int id);
        Task<bool> Add(BookDto newBook);
    }
}