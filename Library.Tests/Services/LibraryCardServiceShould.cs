using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Data;
using Library.Data.Models;
using Library.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Library.Tests.Services
{
    [TestFixture]
    public class LibraryCardServiceShould
    {
        private static IEnumerable<LibraryCard> GetLibraryCards()
        {
            return new List<LibraryCard>
            {
                new LibraryCard
                {
                    Id = 1
                },

                new LibraryCard
                {
                    Id = 2
                }
            };
        }

        private static Mock<DbSet<LibraryCard>> BuildMock()
        {
            var cards = GetLibraryCards().AsQueryable();
            var mockSet = new Mock<DbSet<LibraryCard>>();
            mockSet.As<IQueryable<LibraryCard>>().Setup(b => b.Provider).Returns(cards.Provider);
            mockSet.As<IQueryable<LibraryCard>>().Setup(b => b.Expression).Returns(cards.Expression);
            mockSet.As<IQueryable<LibraryCard>>().Setup(b => b.ElementType).Returns(cards.ElementType);
            mockSet.As<IQueryable<LibraryCard>>().Setup(b => b.GetEnumerator()).Returns(cards.GetEnumerator());
            return mockSet;
        }

        [Test]
        public void Add_New_LibraryCard()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.LibraryCards).Returns(mockSet.Object);
            var sut = new LibraryCardService(mockCtx.Object);

            sut.Add(new LibraryCard());

            mockCtx.Verify(s => s.Add(It.IsAny<LibraryCard>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Test]
        public void Get_All_LibraryCards()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryCards).Returns(mockSet.Object);

            var sut = new LibraryCardService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeOfType(typeof(LibraryCard));
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(a => a.Id == 1);
            queryResult.Should().Contain(a => a.Id == 2);
        }

        [Test]
        public void Get_LibraryCard_By_Id()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryCards).Returns(mockSet.Object);

            var sut = new LibraryCardService(mockCtx.Object);
            var queryResult = sut.Get(2);

            queryResult.Should().BeEquivalentTo(new LibraryCard {Id = 2});
        }
    }
}