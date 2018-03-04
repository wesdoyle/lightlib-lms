using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LibraryData;
using LibraryData.Models;
using LibraryService;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Library.Tests.Services
{
    [TestFixture]
    public class CheckoutServiceShould
    {
        [Test]
        public void Add_Checkout()
        {
            var mockSet = new Mock<DbSet<Checkout>>();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.Checkouts).Returns(mockSet.Object);
            var sut = new CheckoutService(mockCtx.Object);

            sut.Add(new Checkout());

            mockCtx.Verify(s => s.Add(It.IsAny<Checkout>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Test]
        public void Get_Checkout()
        {
            var checkouts = new List<Checkout>()
            {
                new Checkout 
                {
                    Id = 1234,
                    LibraryCard = new LibraryCard()
                    {
                        Id = 1
                    }
                },

                new Checkout 
                {
                    Id = 3214,
                    LibraryCard = new LibraryCard()
                    {
                        Id = 2
                    }
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Checkout>>();
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.Provider).Returns(checkouts.Provider);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.Expression).Returns(checkouts.Expression);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.ElementType).Returns(checkouts.ElementType);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.GetEnumerator()).Returns(checkouts.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Checkouts).Returns(mockSet.Object);

            var sut = new CheckoutService(mockCtx.Object);
            var checkout = sut.Get(3214);

            checkout.LibraryCard.Id.Should().Be(2);
        }

        [Test]
        public void Get_All_Checkouts()
        {
            var checkouts = new List<Checkout>()
            {
                new Checkout 
                {
                    Id = 1234,
                    LibraryCard = new LibraryCard()
                    {
                        Id = 1
                    }
                },

                new Checkout 
                {
                    Id = 3214,
                    LibraryCard = new LibraryCard()
                    {
                        Id = 2
                    }
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Checkout>>();
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.Provider).Returns(checkouts.Provider);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.Expression).Returns(checkouts.Expression);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.ElementType).Returns(checkouts.ElementType);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.GetEnumerator()).Returns(checkouts.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Checkouts).Returns(mockSet.Object);

            var sut = new CheckoutService(mockCtx.Object);
            var result = sut.GetAll().ToList();

            result.Count().Should().Be(2);
            result.Should().Contain(c => c.Id == 3214);
            result.Should().Contain(c => c.Id == 1234);
            result.Should().Contain(c => c.LibraryCard.Id == 1);
            result.Should().Contain(c => c.LibraryCard.Id == 2);
        }

        [Test]
        public void Check_Out_Item_If_Available()
        {
            var item = new Book()
            {
                Id = 8,
                Title = "My Antonia",
                Status = new Status
                {
                    Name = "Available",
                }
            };

            var lc = new LibraryCard()
            {
                Id = 123
            };

            var mockCtx = new Mock<LibraryDbContext>();

            var mockCheckouts = new Mock<DbSet<Checkout>>();
            var mockBooks = new Mock<DbSet<Book>>();
            var mockAssets = new Mock<DbSet<LibraryAsset>>();

            mockCtx.Setup(c => c.Checkouts).Returns(mockCheckouts.Object);
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockAssets.Object);
            mockCtx.Setup(c => c.Books).Returns(mockBooks.Object);

            Assert.Fail("Finish Test");
        }
    }
}