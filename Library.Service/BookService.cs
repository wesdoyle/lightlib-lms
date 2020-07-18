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
    /// Handles business logic for working with Library Books
    /// </summary>
    public class BookService : IBookService { 
        
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<Book> _paginator;

        public BookService(
        LibraryDbContext context, 
        IMapper mapper,
        IPaginator<Book> bookPaginator
        ) {
            _context = context;
            _mapper = mapper;
            _paginator = bookPaginator;
        }

        public async Task<ServiceResult<int>> Add(BookDto newBook) {
            _context.Add(newBook);
            await _context.SaveChangesAsync();
            return new ServiceResult<int> {
                Data = newBook.Id
            };
        }

        public async Task<ServiceResult<BookDto>> Get(int id) {
            var book =  await _context
                .Books.FirstOrDefaultAsync(b => b.Id == id);

            try {
                var bookDto = _mapper.Map<BookDto>(book);

                return new ServiceResult<BookDto> {
                    Data = bookDto,
                    Error = null
                };
            } catch (Exception e) {
                return new ServiceResult<BookDto> {
                    Data = null,
                    Error = new ServiceError {
                        Message = e.Message,
                        Stacktrace = e.StackTrace
                    }
                };
            }
        }

        public async Task<PagedServiceResult<BookDto>> GetAll(int page, int perPage) {
            var books = _context.Books;

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
            var book = await _context.Books.FirstOrDefaultAsync(a => a.ISBN == isbn);
            var bookDto = _mapper.Map<BookDto>(book);
            return new ServiceResult<BookDto> {
                Data = bookDto,
                Error = null
            };
        }
    }
}