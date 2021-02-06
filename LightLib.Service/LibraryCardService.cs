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

namespace LightLib.Service {
    /// <summary>
    /// Handles Library Card business logic
    /// </summary>
    public class LibraryCardService : ILibraryCardService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Paginator<LibraryCard> _paginator;

        public LibraryCardService(
            LibraryDbContext context,
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
            _paginator = new Paginator<LibraryCard>();
        }

        /// <summary>
        /// Gets a paginated collection of Library Cards
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<LibraryCardDto>> GetAll(int page, int perPage) {
            var libraryCards = _context.LibraryCards;
            
            var pageOfCards = await _paginator 
                .BuildPageResult(libraryCards, page, perPage, ch => ch.Created)
                .ToListAsync();

            var paginatedCards = _mapper.Map<List<LibraryCardDto>>(pageOfCards);
            
            return new PaginationResult<LibraryCardDto> {
                Results = paginatedCards,
                PerPage = perPage,
                PageNumber = page
            };
        }

        /// <summary>
        /// Gets a Library Card by ID
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public async Task<LibraryCardDto> Get(int cardId) {
            var libraryCard = await _context.LibraryCards.FirstAsync(p => p.Id == cardId);
            return _mapper.Map<LibraryCardDto>(libraryCard);
        }
        
        /// <summary>
        /// Creates a new Library Card
        /// </summary>
        /// <param name="libraryCardDto"></param>
        /// <returns></returns>
        public async Task<bool> Add(LibraryCardDto libraryCardDto) {
            var newLibraryCard = _mapper.Map<LibraryCard>(libraryCardDto);
            await _context.AddAsync(newLibraryCard);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
