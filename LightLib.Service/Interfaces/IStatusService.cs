using System.Threading.Tasks;
using LightLib.Models.DTOs;
using LightLib.Service.Models;

namespace LightLib.Service.Interfaces {
    public interface IStatusService {
        Task<PagedServiceResult<StatusDto>> GetAll(int page, int perPage);
        Task<ServiceResult<StatusDto>> Get(int statusId);
        Task<ServiceResult<int>> Add(StatusDto status);
    }
}