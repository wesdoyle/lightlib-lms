using System;
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
    /// Handles Library Patron business logic
    /// </summary>
    public class PatronService : IPatronService {
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<Patron> _patronPaginator;
        private readonly IPaginator<CheckoutHistory> _historyPaginator;
        private readonly IPaginator<Checkout> _checkoutPaginator;
        private readonly IPaginator<Hold> _holdPaginator;

        public PatronService(
            LibraryDbContext context,
            IMapper mapper,
            IPaginator<Patron> patronPaginator,
            IPaginator<CheckoutHistory> historyPaginator ,
            IPaginator<Checkout> checkoutPaginator ,
            IPaginator<Hold> holdPaginator
            ) {
            _context = context;
            _mapper = mapper;
            _patronPaginator = patronPaginator;
            _historyPaginator = historyPaginator;
            _holdPaginator = holdPaginator;
            _checkoutPaginator = checkoutPaginator;
        }

        /// <summary>
        /// Gets a Patron by ID
        /// </summary>
        /// <param name="patronId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PatronDto>> Get(int patronId) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstAsync(p => p.Id == patronId);

            var patronDto = _mapper.Map<PatronDto>(patron);
            
            return new ServiceResult<PatronDto> {
                Data = patronDto,
                Error = null
            };
        }

        /// <summary>
        /// Creates a new Patron
        /// </summary>
        /// <param name="newPatronDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> Add(PatronDto newPatronDto) {
            var newPatron = _mapper.Map<Patron>(newPatronDto);
            await _context.AddAsync(newPatron);
            var patronId = await _context.SaveChangesAsync();
            return new ServiceResult<int> {
                Data = patronId,
                Error = null
            };
        }

        /// <summary>
        /// Gets a paginated collection of Patrons
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<PatronDto>> GetAll(int page, int perPage) {
            var patrons = _context.Patrons;

            var pageOfPatrons = await _patronPaginator 
                .BuildPageResult(patrons, page, perPage, b => b.Id)
                .ToListAsync();
            
            var paginatedPatrons = _mapper.Map<List<PatronDto>>(pageOfPatrons);
            
            var paginationResult = new PaginationResult<PatronDto> {
                Results = paginatedPatrons,
                PerPage = perPage,
                PageNumber = page
            };
            
            return new PagedServiceResult<PatronDto> {
                Data = paginationResult,
                Error = null
            };
        }

        /// <summary>
        /// Gets the paginated checkout history of a Patron by ID
        /// </summary>
        /// <param name="patronId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<CheckoutHistoryDto>> GetCheckoutHistory(int patronId, int page,
            int perPage) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Id == patronId);

            if (patron == null) {
                // TODO
                return new PagedServiceResult<CheckoutHistoryDto> {
                    Data = null,
                    Error =  new ServiceError()
                };
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

            var paginationResult = new PaginationResult<CheckoutHistoryDto> {
                Results = paginatedHistory,
                PerPage = perPage,
                PageNumber = page
            };

            return new PagedServiceResult<CheckoutHistoryDto> {
                Data = paginationResult,
                Error = null
            };
        }

        /// <summary>
        /// Gets the Holds currently held by a Patron
        /// </summary>
        /// <param name="patronId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<HoldDto>> GetHolds(int patronId, int page, int perPage) {
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Id == patronId);
                
            if (patron == null) {
                // TODO
                return new PagedServiceResult<HoldDto> {
                    Data = null,
                    Error =  new ServiceError()
                };
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

            var paginationResult = new PaginationResult<HoldDto> {
                Results = paginatedHolds,
                PerPage = perPage,
                PageNumber = page
            };

            return new PagedServiceResult<HoldDto> {
                Data = paginationResult,
                Error = null
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
        public async Task<PagedServiceResult<CheckoutDto>> GetCheckouts(int patronId, int page, int perPage) {
            
            var patron = await _context.Patrons
                .Include(a => a.LibraryCard)
                .FirstAsync(a => a.Id == patronId);
                
            if (patron == null) {
                // TODO
                return new PagedServiceResult<CheckoutDto> {
                    Data = null,
                    Error =  new ServiceError()
                };
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

            var paginationResult = new PaginationResult<CheckoutDto> {
                Results = paginatedCheckouts,
                PerPage = perPage,
                PageNumber = page
            };

            return new PagedServiceResult<CheckoutDto> {
                Data = paginationResult,
                Error = null
            };
        }
    }
}