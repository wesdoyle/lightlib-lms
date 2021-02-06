using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Helpers;
using LightLib.Service.Interfaces;
using LightLib.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Service {
    
    /// <summary>
    /// Handles business logic for working with Library Books
    /// </summary>
    public class BookService : BaseLibraryService, IBookService { 
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Paginator<Book> _paginator;
        
        public BookService(
            LibraryDbContext context, 
            IMapper mapper
        ) {
            _context = context;
            _mapper = mapper;
            _paginator = new Paginator<Book>();
        }

        public async Task<ServiceResult<int>> Add(BookDto newBook) {
            _context.Add(newBook);
            await _context.SaveChangesAsync();
            return new ServiceResult<int> {
                Data = newBook.Id
            };
        }

        /// <summary>
        /// Gets a Book Asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<BookDto>> Get(int id) {
            try {
                var book = await _context.Books.FirstAsync(b => b.Id == id);
                var bookDto = _mapper.Map<BookDto>(book);
                return new ServiceResult<BookDto> {
                    Data = bookDto,
                    Error = null
                };
            } catch (ArgumentNullException ex) {
                return HandleDatabaseError<BookDto>(ex);
            }
        }

        public async Task<PagedServiceResult<BookDto>> GetAll(int page, int perPage) {
            var books = _context.Books;

            try {
                var pageOfBooks = await _paginator
                    .BuildPageResult(books, page, perPage, b => b.Author)
                    .ToListAsync();

                var paginatedBooks = _mapper.Map<List<BookDto>>(pageOfBooks);

                var paginationResult = new PaginationResult<BookDto> {
                    Results = paginatedBooks,
                    PerPage = perPage,
                    PageNumber = page
                };

                return new PagedServiceResult<BookDto> {
                    Data = paginationResult,
                    Error = null
                };
            } catch (Exception ex) {
                return HandleDatabaseCollectionError<BookDto>(ex);
            }
        }

        public async Task<PagedServiceResult<BookDto>> GetByAuthor(
            string author, int page, int perPage) {
            var books = _context.Books;

            try {
                var pageOfBooks = _paginator
                    .BuildPageResult(
                        books,
                        page,
                        perPage,
                        b => b.Author.Contains(author),
                        b => b.Author);

                var paginatedBooks = _mapper
                    .Map<List<BookDto>>(await pageOfBooks.ToListAsync());

                var paginationResult = new PaginationResult<BookDto> {
                    Results = paginatedBooks,
                    PerPage = perPage,
                    PageNumber = page
                };

                return new PagedServiceResult<BookDto> {
                    Data = paginationResult,
                    Error = null
                };
            }
            catch (Exception e) {
                return new PagedServiceResult<BookDto> {
                    Data = null,
                    Error = new ServiceError {
                        Message = e.Message,
                        Stacktrace = e.StackTrace
                    }
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        public async Task<ServiceResult<BookDto>> GetByIsbn(string isbn) {
            var book = await _context.Books.FirstAsync(a => a.ISBN == isbn);
            var bookDto = _mapper.Map<BookDto>(book);
            return new ServiceResult<BookDto> {
                Data = bookDto,
                Error = null
            };
        }
    }
}
