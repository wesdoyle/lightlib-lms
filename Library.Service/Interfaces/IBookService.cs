using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface IBookService {
        Task<PagedServiceResult<BookDto>> GetAll(int page, int pageNumber);
        Task<PagedServiceResult<BookDto>> GetByAuthor(string author, int page, int perPage);
        
        Task<ServiceResult<BookDto>> GetByIsbn(string isbn);
        Task<ServiceResult<BookDto>> Get(int id);
        Task<ServiceResult<int>> Add(BookDto newBook);
    }
}