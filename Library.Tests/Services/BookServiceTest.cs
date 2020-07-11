using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Library.Data;
using Library.Data.Models;
using Library.Models.DTOs;
using Library.Service;
using Library.Service.Interfaces;
using Library.Service.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.Tests.Services {
    public class BookServiceTest {
        [Fact]
        public void Test_Get_Given_Null_Book_Returns_ServiceResult_With_Null_Data() {
            var mockSet = new Mock<DbSet<Book>>();
            var mockMapper = new Mock<IMapper>();
            var mockPaginator = new Mock<IPaginator<Book>>();
            
            // SetupData from package
            
            var books = new List<Book> {
                new Book {
                    Title = "The Waves",
                    Id = 1234
                },
                new Book {
                    Title = "The Snow Leopard",
                    Id = -6
                }
            }.AsQueryable();
            
            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.Provider)
                .Returns(books.Provider);
            
            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.Expression)
                .Returns(books.Expression);
            
            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.ElementType)
                .Returns(books.ElementType);
                
            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.GetEnumerator())
                .Returns(books.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            
            mockCtx
                .Setup(c => c.Books)
                .Returns(mockSet.Object);
            
            var sut = new BookService(
                mockCtx.Object, 
                mockMapper.Object, 
                mockPaginator.Object);
            
            var book = sut.Get(23);
            
            book.Should().BeEquivalentTo(new ServiceResult<BookDto> {
                Data = null,
                Error = null
            });
        }
    }
}