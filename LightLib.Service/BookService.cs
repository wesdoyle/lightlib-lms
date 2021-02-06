using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LightLib.Data;
using LightLib.Data.Models;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.Exceptions;
using LightLib.Service.Helpers;
using LightLib.Service.Interfaces;
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

        public async Task<bool> Add(BookDto newBook) {
            _context.Add(newBook);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets a Book Asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookDto> Get(int id) {
            try {
                var book = await _context.Books.FirstAsync(b => b.Id == id);
                return _mapper.Map<BookDto>(book);
            } catch (Exception ex) {
                throw new LibraryServiceException(Reason.UncaughtError);
            }
        }

        public async Task<PaginationResult<BookDto>> GetAll(int page, int perPage) {
            var books = _context.Books;

            try {
                var pageOfBooks = await _paginator
                    .BuildPageResult(books, page, perPage, b => b.Author)
                    .ToListAsync();

                var paginatedBooks = _mapper.Map<List<BookDto>>(pageOfBooks);

                return new PaginationResult<BookDto> {
                    Results = paginatedBooks,
                    PerPage = perPage,
                    PageNumber = page
                };

            } catch (Exception ex) {
                throw new LibraryServiceException(Reason.UncaughtError);
            }
        }

        public async Task<PaginationResult<BookDto>> GetByAuthor(
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

                return new PaginationResult<BookDto> {
                    Results = paginatedBooks,
                    PerPage = perPage,
                    PageNumber = page
                };
            } catch (Exception e) {
                throw new LibraryServiceException(Reason.UncaughtError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        public async Task<BookDto> GetByIsbn(string isbn) {
            try {
                var book = await _context.Books.FirstAsync(a => a.ISBN == isbn);
                return _mapper.Map<BookDto>(book);
            } catch (Exception e) {
                throw new LibraryServiceException(Reason.UncaughtError);
            }
        }
    }
}
