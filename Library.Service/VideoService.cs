using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Library.Models.DTOs;
using Library.Service.Interfaces;
using Library.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Service {
    
    /// <summary>
    /// Handles Video (Library Asset) business logic 
    /// </summary>
    public class VideoService : IVideoService {
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<Video> _paginator;

        public VideoService(LibraryDbContext context, IMapper mapper, IPaginator<Video> paginator) {
            _context = context;
            _mapper = mapper;
            _paginator= paginator;
        }

        /// <summary>
        /// Gets a paginated collection of Videos 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<VideoDto>> GetAll(int page, int perPage) {
            var videos = _context.Videos;

            var pageOfVideos = await _paginator 
                .BuildPageResult(videos, page, perPage, b => b.Id)
                .ToListAsync();
            
            var paginatedVideos = _mapper.Map<List<VideoDto>>(pageOfVideos);
            
            var paginationResult = new PaginationResult<VideoDto> {
                Results = paginatedVideos,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<VideoDto> {
                Data = paginationResult,
                Error = null
            };
        }

        /// <summary>
        /// Gets a paginated list of Videos by a given Director
        /// </summary>
        /// <param name="director"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<VideoDto>> GetByDirector(string director, int page, int perPage) {
            var videos = _context.Videos.Where(v => v.Director.Contains(director));

            var pageOfVideos = await _paginator 
                .BuildPageResult(videos, page, perPage, b => b.Id)
                .ToListAsync();
            
            var paginatedVideos = _mapper.Map<List<VideoDto>>(pageOfVideos);
            
            var paginationResult = new PaginationResult<VideoDto> {
                Results = paginatedVideos,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<VideoDto> {
                Data = paginationResult,
                Error = null
            };
        }

        /// <summary>
        /// Gets a Video Library Asset by ID
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<VideoDto>> Get(int videoId) {
            var video = await _context.Videos
                .FirstAsync(p => p.Id == videoId);

            var videoDto = _mapper.Map<VideoDto>(video);
            
            return new ServiceResult<VideoDto> {
                Data = videoDto,
                Error = null
            };
        }

        /// <summary>
        /// Creates a new Video Library Asset
        /// </summary>
        /// <param name="videoDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> Add(VideoDto videoDto) {

            var video = _mapper.Map<Video>(videoDto);
                        
            await _context.AddAsync(video);
            
            var videoId = await _context.SaveChangesAsync();
            
            return new ServiceResult<int> {
                Data = videoId,
                Error = null
            };
        }
    }
}