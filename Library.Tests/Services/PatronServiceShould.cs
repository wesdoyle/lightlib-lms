using System;
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
    public class PatronServiceShould
    {
        private static IEnumerable<Patron> GetPatrons()
        {
            return new List<Patron>
            {
                new Patron
                {
                    Id = 1,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Telephone = "1234",
                    Address = "1 Main St",
                    DateOfBirth = new DateTime(1972, 01, 23)
                },

                new Patron
                {
                    Id = 2,
                    FirstName = "Jack",
                    LastName = "Smith",
                    Telephone = "233-4411",
                    Address = "Oak Drive",
                    DateOfBirth = new DateTime(1983, 07, 3)
                }
            };
        }

        [Test]
        public void Add_New_Patron()
        {
            var mockSet = new Mock<DbSet<Patron>>();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.Patrons).Returns(mockSet.Object);
            var sut = new PatronService(mockCtx.Object);

            sut.Add(new Patron());

            mockCtx.Verify(s => s.Add(It.IsAny<Patron>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Test]
        public void Get_All_Patrons()
        {
            var patrons = GetPatrons().AsQueryable();

            var mockSet = new Mock<DbSet<Patron>>();
            mockSet.As<IQueryable<Patron>>().Setup(b => b.Provider).Returns(patrons.Provider);
            mockSet.As<IQueryable<Patron>>().Setup(b => b.Expression).Returns(patrons.Expression);
            mockSet.As<IQueryable<Patron>>().Setup(b => b.ElementType).Returns(patrons.ElementType);
            mockSet.As<IQueryable<Patron>>().Setup(b => b.GetEnumerator()).Returns(patrons.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Patrons).Returns(mockSet.Object);

            var sut = new PatronService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeOfType(typeof(Patron));
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(a => a.FirstName == "Jane");
            queryResult.Should().Contain(a => a.FirstName == "Jack");
        }

        [Test]
        public void Get_Patron_By_Id()
        {
            var patrons = GetPatrons().AsQueryable();

            var mockSet = new Mock<DbSet<Patron>>();
            mockSet.As<IQueryable<Patron>>().Setup(b => b.Provider).Returns(patrons.Provider);
            mockSet.As<IQueryable<Patron>>().Setup(b => b.Expression).Returns(patrons.Expression);
            mockSet.As<IQueryable<Patron>>().Setup(b => b.ElementType).Returns(patrons.ElementType);
            mockSet.As<IQueryable<Patron>>().Setup(b => b.GetEnumerator()).Returns(patrons.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Patrons).Returns(mockSet.Object);

            var sut = new PatronService(mockCtx.Object);
            var branch = sut.Get(1);

            branch.FirstName.Should().Be("Jane");
        }

        [Test]
        public void Get_Patron_Checkout_History()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_patron_checkout_history")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 86,
                    Title = "The Enchiridion"
                };

                var video = new Video
                {
                    Id = 2193,
                    Title = "Blade Runner"
                };

                var card = new LibraryCard
                {
                    Id = 82
                };

                var patron = new Patron
                {
                    Id = 7,
                    LibraryCard = card
                };

                context.Patrons.Add(patron);
                context.Videos.Add(video);
                context.Books.Add(book);
                context.SaveChanges();

                var checkoutHistories = new List<CheckoutHistory>
                {
                    new CheckoutHistory
                    {
                        Id = 1,
                        LibraryCard = card,
                        LibraryAsset = book
                    },
                    new CheckoutHistory
                    {
                        Id = 2,
                        LibraryCard = card,
                        LibraryAsset = video
                    }
                };

                context.CheckoutHistories.AddRange(checkoutHistories);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new PatronService(context);
                var result = sut.GetCheckoutHistory(7);
                var checkoutHistories = result as CheckoutHistory[] ?? result.ToArray();
                checkoutHistories.Length.Should().Be(2);
            }
        }

        [Test]
        public void Get_Patron_Checkouts()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_patron_checkouts")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 231,
                    Title = "Compilers - Principles, Techniques, and Tools"
                };

                var video = new Video
                {
                    Id = 8878,
                    Title = "The Great British Baking Show"
                };

                var card = new LibraryCard
                {
                    Id = 801
                };

                var patron = new Patron
                {
                    Id = 72,
                    LibraryCard = card
                };

                context.Patrons.Add(patron);
                context.Videos.Add(video);
                context.Books.Add(book);
                context.SaveChanges();

                var checkouts = new List<Checkout>
                {
                    new Checkout
                    {
                        Id = 1992,
                        LibraryCard = card,
                        LibraryAsset = book
                    },
                    new Checkout
                    {
                        Id = 1993,
                        LibraryCard = card,
                        LibraryAsset = video
                    },
                    new Checkout
                    {
                        Id = 1994,
                        LibraryCard = card,
                        LibraryAsset = new Book {Title = "Frankenstein "}
                    }
                };

                context.Checkouts.AddRange(checkouts);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new PatronService(context);
                var result = sut.GetCheckouts(72);
                var checkouts = result as Checkout[] ?? result.ToArray();
                checkouts.Length.Should().Be(3);
            }
        }

        [Test]
        public void Get_Patron_Holds()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_patron_holds")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var card = new LibraryCard
                {
                    Id = 83
                };

                var hold = new Hold
                {
                    Id = 56,
                    LibraryAsset = new Book {Id = 1},
                    LibraryCard = card
                };

                var patron = new Patron
                {
                    Id = 8,
                    LibraryCard = card
                };

                context.Patrons.Add(patron);
                context.Holds.Add(hold);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new PatronService(context);
                var result = sut.GetHolds(8);
                result.Count().Should().Be(1);
            }
        }
    }
}