using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Helpers;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Assets {
    
    /// <summary>
    /// Handles Asset Status business logic
    /// </summary>
    public class StatusService : IStatusService {
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Paginator<AvailabilityStatus> _paginator;

        public StatusService(
            LibraryDbContext context,
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
            _paginator = new Paginator<AvailabilityStatus>();
        }

        /// <summary>
        /// Creates a new Status
        /// </summary>
        /// <param name="statusDto"></param>
        /// <returns></returns>
        public async Task<bool> Add(StatusDto statusDto) {
            var status = _mapper.Map<AvailabilityStatus>(statusDto);
            await _context.AddAsync(status);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets a paginated collection of Statuses
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<StatusDto>> GetAll(int page, int perPage) {
            var statuses = _context.Statuses;

            var pageOfStatuses = await _paginator 
                .BuildPageResult(statuses, page, perPage, b => b.Id)
                .ToListAsync();
            
            var paginatedStatuses = _mapper.Map<List<StatusDto>>(pageOfStatuses);
            
            return new PaginationResult<StatusDto> {
                Results = paginatedStatuses,
                PerPage = perPage,
                PageNumber = page
            };
        }

        /// <summary>
        /// Gets a Status by Id
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public async Task<StatusDto> Get(int statusId) {
            var status = await _context.Statuses.FirstAsync(p => p.Id == statusId);
            return _mapper.Map<StatusDto>(status);
        }
    }
}
