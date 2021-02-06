using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface IPatronService {
        Task<PaginationResult<PatronDto>> GetAll(int page, int perPage);
        Task<PaginationResult<CheckoutHistoryDto>> GetCheckoutHistory(int patronId, int page, int perPage);
        Task<PaginationResult<HoldDto>> GetHolds(int patronId, int page, int perPage);
        Task<PaginationResult<CheckoutDto>> GetCheckouts(int patronId, int page, int perPage);
        Task<PatronDto> Get(int patronId);
        Task<bool> Add(PatronDto newPatron);
    }
}