using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Patrons {
    public class PatronService : IPatronService {
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public PatronService(
            LibraryDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PatronDto> Get(int patronId) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstAsync(p => p.Id == patronId);

            return _mapper.Map<PatronDto>(patron);
        }

        public async Task<bool> Add(PatronDto newPatronDto) {
            var newPatron = _mapper.Map<Patron>(newPatronDto);
            await _context.AddAsync(newPatron);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<PaginationResult<PatronDto>> GetPaginated(int page, int perPage) {
            var patrons = _context.Patrons;
            var pageOfPatrons = await patrons.ToPaginatedResult(page, perPage);
            var pageOfPatronDtos = _mapper.Map<List<PatronDto>>(pageOfPatrons.Results);
            return new PaginationResult<PatronDto> {
                    PageNumber = pageOfPatrons.PageNumber,
                    PerPage = pageOfPatrons.PerPage,
                    Results = pageOfPatronDtos 
            };
        }

        public async Task<PaginationResult<CheckoutHistoryDto>> GetPaginatedCheckoutHistory(int patronId, int page, int perPage) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Id == patronId);

            if (patron == null) {
                // TODO
                return new PaginationResult<CheckoutHistoryDto>();
            }

            var cardId = patron.LibraryCard.Id;

            var histories = _context.CheckoutHistories
                .Include(a => a.LibraryCard)
                .Include(a => a.Asset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.CheckedOut);
            
            var pageOfHistory = await histories.ToPaginatedResult(page, perPage);
            var pageOfHistoryDto = _mapper.Map<List<CheckoutHistoryDto>>(pageOfHistory.Results);
            return new PaginationResult<CheckoutHistoryDto> {
                    PageNumber = pageOfHistory.PageNumber,
                    PerPage = pageOfHistory.PerPage,
                    Results = pageOfHistoryDto 
            };
        }

        public async Task<PaginationResult<HoldDto>> GetPaginatedHolds(int patronId, int page, int perPage) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Id == patronId);
                
            if (patron == null) {
                // TODO
            }
                
            var libraryCardId = patron.LibraryCard.Id;

            var holds = _context.Holds
                .Include(a => a.LibraryCard)
                .Include(a => a.Asset)
                .Where(a => a.LibraryCard.Id == libraryCardId)
                .OrderByDescending(a => a.HoldPlaced);

            var pageOfHolds = await holds.ToPaginatedResult(page, perPage);
            var pageOfHoldsDto = _mapper.Map<List<HoldDto>>(pageOfHolds.Results);
            return new PaginationResult<HoldDto> {
                    PageNumber = pageOfHolds.PageNumber,
                    PerPage = pageOfHolds.PerPage,
                    Results = pageOfHoldsDto 
            };
        }

        public async Task<PaginationResult<CheckoutDto>> GetPaginatedCheckouts(int patronId, int page, int perPage) {
            
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Id == patronId);
                
            if (patron == null) {
                // TODO
                return new PaginationResult<CheckoutDto>();
            }
                
            var libraryCardId = patron.LibraryCard.Id;

            var checkouts = _context.Checkouts
                .Include(a => a.LibraryCard)
                .Include(a => a.Asset)
                .Where(a => a.LibraryCard.Id == libraryCardId)
                .OrderByDescending(a => a.CheckedOutSince);

            var pageOfCheckouts = await checkouts.ToPaginatedResult(page, perPage);
            var pageOfCheckoutsDto = _mapper.Map<List<CheckoutDto>>(pageOfCheckouts.Results);
            return new PaginationResult<CheckoutDto> {
                    PageNumber = pageOfCheckouts.PageNumber,
                    PerPage = pageOfCheckouts.PerPage,
                    Results = pageOfCheckoutsDto 
            };
        }
    }
}
