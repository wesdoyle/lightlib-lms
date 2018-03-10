using System;
using System.Collections.Generic;
using Library.Data;
using System.Linq;
using FluentAssertions;
using Library.Data.Models;
using Library.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Library.Tests.Services
{
    [TestFixture]
    public class CheckoutServiceShould
    {
        [Test]
        public void Add_New_Checkout()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.Add(new Checkout
                {
                    Id = -247
                });
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                Assert.AreEqual(1, context.Checkouts.Count());
                Assert.AreEqual(-247, context.Checkouts.Single().Id);
            }
        }

        [Test]
        public void Add_New_Checkout_Calls_Context_Save()
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
        public void Get_All_Checkouts()
        {
            var cos = GetCheckouts().AsQueryable();

            var mockSet = new Mock<DbSet<Checkout>>();
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.Provider).Returns(cos.Provider);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.Expression).Returns(cos.Expression);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.ElementType).Returns(cos.ElementType);
            mockSet.As<IQueryable<Checkout>>().Setup(b => b.GetEnumerator()).Returns(cos.GetEnumerator());

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Checkouts).Returns(mockSet.Object);

            var sut = new CheckoutService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeOfType(typeof(Checkout));
            queryResult.Should().HaveCount(3);
            queryResult.Should().Contain(a => a.LibraryCard.Id == -14);
        }

        private static Checkout GetCheckout()
        {
            return new Checkout
            {
                Id = 304,
                Since = new DateTime(2018, 03, 09),
                Until = new DateTime(2018, 04, 12),
                LibraryCard = new LibraryCard()
                {
                    Id = -1,
                    Created = new DateTime(2008, 01, 21)
                }
            };
        }

        private static IEnumerable<Checkout> GetCheckouts()
        {
            return new List<Checkout>()
            {
                new Checkout
                {
                    Id = 1234,
                    Since = new DateTime(2018, 03, 09),
                    Until = new DateTime(2018, 04, 12),
                    LibraryCard = new LibraryCard()
                    {
                        Id = -1,
                        Created = new DateTime(2008, 01, 21),
                        Fees = 123M
                    }
                },
                new Checkout
                {
                    Id = 999,
                    Since = new DateTime(2018, 03, 09),
                    Until = new DateTime(2018, 04, 12),
                    LibraryCard = new LibraryCard()
                    {
                        Id = -14,
                        Created = new DateTime(2008, 01, 21),
                        Fees = 123M
                    }
                },
                new Checkout
                {
                    Id = 416,
                    Since = new DateTime(2017, 03, 09),
                    Until = new DateTime(2017, 05, 24),
                    LibraryCard = new LibraryCard()
                    {
                        Id = 824,
                        Created = new DateTime(1994, 05, 16),
                        Fees = 123M
                    }
                },
            };
        }

        [Test]
        public void Get_Checkout() { }

        [Test]
        public void Checkout_Item() { }

        [Test]
        public void Mark_Item_Lost() { }

        [Test]
        public void Mark_Item_Found() { }

        [Test]
        public void Place_Hold() { }

        [Test]
        public void Check_In_Item() { }

        [Test]
        public void Get_Checkout_History() { }

        [Test]
        public void Get_Latest_Checkout() { }

        [Test]
        public void Get_Available_Copies() { }

        [Test]
        public void Get_Number_Of_Copies() { }

        [Test]
        public void Get_IsCheckedOut() { }

        [Test]
        public void Get_Curren_tHold_Patron() { }

        [Test]
        public void Get_Current_Holds() { }

        [Test]
        public void Get_CurrentHoldPlaced() { }

        [Test]
        public void Get_Current_Patron() { }

        [Test]
        public void Checks_Out_To_Earliest_Hold_When_Checked_In() { }
    }
}
