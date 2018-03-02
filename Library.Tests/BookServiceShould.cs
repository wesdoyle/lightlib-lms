using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LibraryData;
using LibraryData.Models;
using LibraryService;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Library.Tests
{
    [TestFixture]
    public class BookServiceShould
    {
        [Test]
        public void Add_NewBook_To_Context()
        {
            var mockSet = new Mock<DbSet<Book>>();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);
            var sut = new BookService(mockCtx.Object);

            sut.Add(new Book
            {
                Author = "Virginia Woolf",
                Cost = 12.00M,
                Title = "The Waves",
                DeweyIndex = "ABC",
                Id = 1234,
            });

            mockCtx.Verify(s => s.Add(It.IsAny<Book>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Test]
        public void Get_Book_From_Context()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Author = "Virginia Woolf",
                    Cost = 12.00M,
                    Title = "The Waves",
                    DeweyIndex = "ABC",
                    Id = 1234,
                },

                new Book
                {
                    Author = "Peter Matthiessen",
                    Cost = 11.00M,
                    Title = "The Snow Leopard",
                    DeweyIndex = "SNO",
                    Id = -6,
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IQueryable<Book>>().Setup(b => b.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Book>>().Setup(b => b.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Book>>().Setup(b => b.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(b => b.GetEnumerator()).Returns(books.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);

            var sut = new BookService(mockCtx.Object);
            var book = sut.Get(-6);

            book.Title.Should().Be("The Snow Leopard");
        }

        [Test]
        public void Return_All_Books()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Author = "Virginia Woolf",
                    Cost = 12.00M,
                    Title = "The Waves",
                    DeweyIndex = "ABC",
                    Id = 1234,
                },

                new Book
                {
                    Author = "Peter Matthiessen",
                    Cost = 11.00M,
                    Title = "The Snow Leopard",
                    DeweyIndex = "SNO",
                    Id = -6,
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IQueryable<Book>>().Setup(b => b.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Book>>().Setup(b => b.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Book>>().Setup(b => b.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(b => b.GetEnumerator()).Returns(books.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);

            var sut = new BookService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeOfType(typeof(Book));
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(a => a.DeweyIndex == "SNO");
        }
    }
}
