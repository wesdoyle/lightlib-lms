using System.Collections.Generic;
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
    /// Handles Library Card business logic
    /// </summary>
    public class LibraryCardService : ILibraryCardService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<LibraryCard> _paginator;

        public LibraryCardService(
            LibraryDbContext context,
            IMapper mapper,
            IPaginator<LibraryCard> paginator
            ) {
            _context = context;
            _paginator = paginator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a paginated collection of Library Cards
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<LibraryCardDto>> GetAll(int page, int perPage) {
            var libraryCards = _context.LibraryCards;
            
            var pageOfCards = await _paginator 
                .BuildPageResult(libraryCards, page, perPage, ch => ch.Created)
                .ToListAsync();

            var paginatedCards = _mapper.Map<List<LibraryCardDto>>(pageOfCards);
            
            var paginationResult = new PaginationResult<LibraryCardDto> {
                Results = paginatedCards,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<LibraryCardDto> {
                Data = paginationResult,
                Error = null
            };
        }

        /// <summary>
        /// Gets a Library Card by ID
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LibraryCardDto>> Get(int cardId) {
            var libraryCard = await _context.LibraryCards.FirstAsync(p => p.Id == cardId);
            var libraryCardDto = _mapper.Map<LibraryCardDto>(libraryCard);
            return new ServiceResult<LibraryCardDto> {
                Data = libraryCardDto,
                Error = null
            };
        }
        
        /// <summary>
        /// Creates a new Library Card
        /// </summary>
        /// <param name="libraryCardDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> Add(LibraryCardDto libraryCardDto) {
            var newLibraryCard = _mapper.Map<LibraryCard>(libraryCardDto);
            await _context.AddAsync(newLibraryCard);
            var newCardId = await _context.SaveChangesAsync();
            
            return new ServiceResult<int> {
                Data = newCardId,
                Error = null
            };
        }
    }
}