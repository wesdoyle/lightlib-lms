using System.Threading.Tasks;
using Library.Models.DTOs;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface IVideoService {
        Task<PagedServiceResult<VideoDto>> GetAll(int page, int perPage);
        Task<PagedServiceResult<VideoDto>> GetByDirector(string author, int page, int perPage);
        Task<ServiceResult<VideoDto>> Get(int videoId);
        Task<ServiceResult<int>> Add(VideoDto video);
    }
}