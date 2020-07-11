using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data;
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

        public BookService(LibraryDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
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
            
            return new ServiceResult<BookDto> {
                Data = new BookDto {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    ImageUrl = book.ImageUrl
                }
            };
        }

        public async Task<PagedServiceResult<BookDto>> GetAll() {
            var books = _context.Books;
            // implement pagination
            var pagedBooks = new PaginationResult<BookDto>();
            return new PagedServiceResult<BookDto> {
                Data = pagedBooks
            };
        }

        public async Task<PagedServiceResult<BookDto>> GetByAuthor(string author) {
            var books = await _context.Books
                .Where(a => a.Author.Contains(author))
                .ToListAsync();
            var pagedBooks = new PaginationResult<BookDto>();
            return new PagedServiceResult<BookDto> {
                Data = pagedBooks
            };
        }

        public async Task<PagedServiceResult<BookDto>> GetByIsbn(string isbn) {
            var books = await _context.Books
                .Where(a => a.ISBN == isbn)
                .ToListAsync();
            var pagedBooks = new PaginationResult<BookDto>();
            return new PagedServiceResult<BookDto> {
                Data = pagedBooks
            };
        }
    }
}