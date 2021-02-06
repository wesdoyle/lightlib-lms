using System.Threading.Tasks;
using LightLib.Models.DTOs;
using LightLib.Service.Models;

namespace LightLib.Service.Interfaces {
    public interface IVideoService {
        Task<PagedServiceResult<VideoDto>> GetAll(int page, int perPage);
        Task<PagedServiceResult<VideoDto>> GetByDirector(string director, int page, int perPage);
        Task<ServiceResult<VideoDto>> Get(int videoId);
        Task<ServiceResult<int>> Add(VideoDto videoDto);
    }
}