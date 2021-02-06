using System.Threading.Tasks;
using LightLib.Models.DTOs;
using LightLib.Service.Models;

namespace LightLib.Service.Interfaces {
    public interface IPatronService {
        Task<PagedServiceResult<PatronDto>> GetAll(int page, int perPage);
        Task<PagedServiceResult<CheckoutHistoryDto>> GetCheckoutHistory(
            int patronId, int page, int perPage);
        Task<PagedServiceResult<HoldDto>> GetHolds(
            int patronId, int page, int perPage);
        Task<PagedServiceResult<CheckoutDto>> GetCheckouts(
            int patronId, int page, int perPage);
        Task<ServiceResult<PatronDto>> Get(int patronId);
        Task<ServiceResult<int>> Add(PatronDto newPatron);
    }
}