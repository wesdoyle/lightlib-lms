using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Helpers;
using LightLib.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service.Patrons {
    /// <summary>
    /// Handles Library Patron business logic
    /// </summary>
    public class PatronService : IPatronService {
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Paginator<Patron> _patronPaginator;
        private readonly Paginator<CheckoutHistory> _historyPaginator;
        private readonly Paginator<Data.Models.Checkout> _checkoutPaginator;
        private readonly Paginator<Hold> _holdPaginator;

        public PatronService(
            LibraryDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;
            _patronPaginator = new Paginator<Patron>();
            _historyPaginator = new Paginator<CheckoutHistory>();
            _holdPaginator = new Paginator<Hold>();
            _checkoutPaginator = new Paginator<Data.Models.Checkout>();
        }

        /// <summary>
        /// Gets a Patron by ID
        /// </summary>
        /// <param name="patronId"></param>
        /// <returns></returns>
        public async Task<PatronDto> Get(int patronId) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstAsync(p => p.Id == patronId);

            return _mapper.Map<PatronDto>(patron);
        }

        /// <summary>
        /// Creates a new Patron
        /// </summary>
        /// <param name="newPatronDto"></param>
        /// <returns></returns>
        public async Task<bool> Add(PatronDto newPatronDto) {
            var newPatron = _mapper.Map<Patron>(newPatronDto);
            await _context.AddAsync(newPatron);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets a paginated collection of Patrons
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<PatronDto>> GetAll(int page, int perPage) {
            var patrons = _context.Patrons;

            var pageOfPatrons = await _patronPaginator 
                .BuildPageResult(patrons, page, perPage, b => b.Id)
                .ToListAsync();
            
            var paginatedPatrons = _mapper.Map<List<PatronDto>>(pageOfPatrons);
            
            return new PaginationResult<PatronDto> {
                Results = paginatedPatrons,
                PerPage = perPage,
                PageNumber = page
            };
        }

        /// <summary>
        /// Gets the paginated checkout history of a Patron by ID
        /// </summary>
        /// <param name="patronId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<CheckoutHistoryDto>> GetCheckoutHistory(int patronId, int page,
            int perPage) {
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
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.CheckedOut);

            var pageOfHistories = await _historyPaginator
                .BuildPageResult(histories, page, perPage, b => b.CheckedOut)
                .ToListAsync();

            var paginatedHistory = _mapper.Map<List<CheckoutHistoryDto>>(pageOfHistories);

            return new PaginationResult<CheckoutHistoryDto> {
                Results = paginatedHistory,
                PerPage = perPage,
                PageNumber = page
            };
        }

        /// <summary>
        /// Gets the Holds currently held by a Patron
        /// </summary>
        /// <param name="patronId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PaginationResult<HoldDto>> GetHolds(int patronId, int page, int perPage) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Id == patronId);
                
            if (patron == null) {
                // TODO
            }
                
            var libraryCardId = patron.LibraryCard.Id;

            var holds = _context.Holds
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == libraryCardId)
                .OrderByDescending(a => a.HoldPlaced);

            var pageOfHolds = await _holdPaginator 
                .BuildPageResult(holds, page, perPage, b => b.Id)
                .ToListAsync();

            var paginatedHolds = _mapper.Map<List<HoldDto>>(pageOfHolds);

            return new PaginationResult<HoldDto> {
                Results = paginatedHolds,
                PerPage = perPage,
                PageNumber = page
            };
        }

        /// <summary>
        /// Gets a paginated collection of the provided Patron's Checkouts 
        /// </summary>
        /// <param name="patronId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PaginationResult<CheckoutDto>> GetCheckouts(int patronId, int page, int perPage) {
            
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
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == libraryCardId)
                .OrderByDescending(a => a.Since);

            var pageOfCheckouts = await _checkoutPaginator 
                .BuildPageResult(checkouts, page, perPage, b => b.Id)
                .ToListAsync();

            var paginatedCheckouts = _mapper.Map<List<CheckoutDto>>(pageOfCheckouts);

            return new PaginationResult<CheckoutDto> {
                Results = paginatedCheckouts,
                PerPage = perPage,
                PageNumber = page
            };
        }
    }
}
