using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Assets {
    
    public class StatusService : IStatusService {
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public StatusService(
            LibraryDbContext context,
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Add(StatusDto statusDto) {
            var status = _mapper.Map<AvailabilityStatus>(statusDto);
            await _context.AddAsync(status);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaginationResult<StatusDto>> GetPaginated(int page, int perPage) {
            var statuses = _context.Statuses;
            var pageOfStatuses = await statuses.ToPaginatedResult(page, perPage);
            var pageOfAssetDtos = _mapper.Map<List<StatusDto>>(pageOfStatuses.Results);
            return new PaginationResult<StatusDto> {
                    PageNumber = pageOfStatuses.PageNumber,
                    PerPage = pageOfStatuses.PerPage,
                    Results = pageOfAssetDtos 
            };
        }

        public async Task<StatusDto> Get(int statusId) {
            var status = await _context.Statuses.FirstAsync(p => p.Id == statusId);
            return _mapper.Map<StatusDto>(status);
        }
    }
}
