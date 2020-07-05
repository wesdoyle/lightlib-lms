using System.Threading.Tasks;
using Library.Models;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface IBookService {
        Task<PagedServiceResult<BookDto>> GetAll();
        Task<PagedServiceResult<BookDto>> GetByAuthor(string author);
        Task<PagedServiceResult<BookDto>> GetByIsbn(string isbn);
        Task<ServiceResult<BookDto>> Get(int id);
        Task<ServiceResult<int>> Add(BookDto newBook);
    }
}