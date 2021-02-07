using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface IStatusService {
        Task<PaginationResult<StatusDto>> GetPaginated(int page, int perPage);
        Task<StatusDto> Get(int statusId);
        Task<bool> Add(StatusDto status);
    }
}