using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface IStatusService {
        Task<PagedServiceResult<StatusDto>> GetAll(int page, int perPage);
        Task<ServiceResult<StatusDto>> Get(int statusId);
        Task<ServiceResult<int>> Add(StatusDto status);
    }
}