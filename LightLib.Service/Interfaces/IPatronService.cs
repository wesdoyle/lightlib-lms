using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface IPatronService {
        Task<PaginationResult<PatronDto>> GetPaginated(int page, int perPage);
        Task<PaginationResult<CheckoutHistoryDto>> GetPaginatedCheckoutHistory(int patronId, int page, int perPage);
        Task<PaginationResult<HoldDto>> GetPaginatedHolds(int patronId, int page, int perPage);
        Task<PaginationResult<CheckoutDto>> GetPaginatedCheckouts(int patronId, int page, int perPage);
        Task<PatronDto> Get(int patronId);
        Task<bool> Add(PatronDto newPatron);
    }
}