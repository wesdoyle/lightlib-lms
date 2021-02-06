using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Service.Interfaces {
    public interface IVideoService {
        Task<PaginationResult<VideoDto>> GetAll(int page, int perPage);
        Task<PaginationResult<VideoDto>> GetByDirector(string director, int page, int perPage);
        Task<VideoDto> Get(int videoId);
        Task<bool> Add(VideoDto videoDto);
    }
}