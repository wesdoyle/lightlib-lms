using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Patrons {
    public class LibraryCardService : ILibraryCardService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public LibraryCardService(
            LibraryDbContext context,
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationResult<LibraryCardDto>> GetPaginated(int page, int perPage) {
            var libraryCards = _context.LibraryCards;
            var pageOfLibraryCards = await libraryCards.ToPaginatedResult(page, perPage);
            var pageOfLibraryCardsDto = _mapper.Map<List<LibraryCardDto>>(pageOfLibraryCards.Results);
            return new PaginationResult<LibraryCardDto> {
                    PageNumber = pageOfLibraryCards.PageNumber,
                    PerPage = pageOfLibraryCards.PerPage,
                    Results = pageOfLibraryCardsDto 
            };
        }

        public async Task<LibraryCardDto> Get(int cardId) {
            var libraryCard = await _context.LibraryCards.FirstAsync(p => p.Id == cardId);
            return _mapper.Map<LibraryCardDto>(libraryCard);
        }

        public async Task<bool> Add(LibraryCardDto libraryCardDto) {
            var newLibraryCard = _mapper.Map<LibraryCard>(libraryCardDto);
            await _context.AddAsync(newLibraryCard);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
