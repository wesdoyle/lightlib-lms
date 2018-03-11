using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class CheckoutServiceShould
    {
        private static Checkout GetCheckout()
        {
            return new Checkout
            {
                Id = 304,
                Since = new DateTime(2018, 03, 09),
                Until = new DateTime(2018, 04, 12),
                LibraryCard = new LibraryCard
                {
                    Id = -1,
                    Created = new DateTime(2008, 01, 21)
                }
            };
        }

        private static IEnumerable<Checkout> GetCheckouts()
        {
            return new List<Checkout>
            {
                new Checkout
                {
                    Id = 1234,
                    Since = new DateTime(2018, 03, 09),
                    Until = new DateTime(2018, 04, 12),
                    LibraryCard = new LibraryCard
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
                    LibraryCard = new LibraryCard
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
                    LibraryCard = new LibraryCard
                    {
                        Id = 824,
                        Created = new DateTime(1994, 05, 16),
                        Fees = 123M
                    }
                }
            };
        }

        [Test]
        public void Add_Checkout_To_Database()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Adds_checkout_to_database")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Checkouts.Add(new Checkout {Id = -982});
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.Get(-982);
                context.Checkouts.Count().Should().Be(1);
            }
        }

        [Test]
        public void Add_New_Checkout()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Add_writes_to_database")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.Add(new Checkout
                {
                    Id = -247
                });
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
        public void Check_In_Item()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Checks_In_Item")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Statuses.Add(new Status
                {
                    Name = "Available"
                });

                context.LibraryAssets.Add(new Book
                {
                    Id = -516,
                    Status = new Status
                    {
                        Id = 2,
                        Name = "Checked Out"
                    }
                });

                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.CheckInItem(-516);
                var book = context.LibraryAssets.Find(-516);
                book.Status.Name.Should().Be("Available");
            }
        }

        [Test]
        public void Checks_Out_To_Earliest_Hold_When_Checked_In()
        {
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


        [Test]
        public void Get_Checkout()
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
            var queryResult = sut.Get(1234);

            queryResult.Should().BeOfType(typeof(Checkout));
            queryResult.LibraryCard.Id.Should().Be(-1);
        }

        [Test]
        public void Get_Checkout_History()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_checkout_history")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 14,
                    Title = "Man and His Symbols"
                };

                context.SaveChanges();

                var checkoutHistories = new List<CheckoutHistory>
                {
                    new CheckoutHistory
                    {
                        Id = 1,
                        LibraryCard = new LibraryCard
                        {
                            Id = 64
                        },
                        LibraryAsset = book
                    },
                    new CheckoutHistory
                    {
                        Id = 2,
                        LibraryCard = new LibraryCard
                        {
                            Id = 182
                        },
                        LibraryAsset = book
                    }
                };

                context.CheckoutHistories.AddRange(checkoutHistories);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                var result = sut.GetCheckoutHistory(14);
                result.Count().Should().Be(2);
            }
        }

        [Test]
        public void Get_Current_Hold_Patron()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_checked_out_item")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 2319,
                    Title = "Ulysses",
                    NumberOfCopies = 99
                };

                var card = new LibraryCard
                {
                    Id = 16
                };

                context.Books.Add(book);
                context.LibraryCards.Add(card);
                context.SaveChanges();

                var patron = new Patron
                {
                    Id = 118,
                    LibraryCard = card,
                    FirstName = "Frodo",
                    LastName = "Baggins"
                };

                var hold = new Hold
                {
                    Id = 41,
                    LibraryAsset = book,
                    LibraryCard = card
                };

                context.Patrons.Add(patron);
                context.Holds.Add(hold);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                var result = sut.GetCurrentHoldPatron(41);
                result.Should().Be("Frodo Baggins");
            }
        }

        [Test]
        public void Get_Current_Holds()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_current_holds")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 2319,
                    Title = "Ulysses",
                    NumberOfCopies = 99
                };

                var card = new LibraryCard
                {
                    Id = 16
                };

                context.Books.Add(book);
                context.LibraryCards.Add(card);
                context.SaveChanges();

                var patron = new Patron
                {
                    Id = 118,
                    LibraryCard = card
                };

                var hold = new Hold
                {
                    Id = 41,
                    LibraryAsset = book,
                    LibraryCard = card
                };

                context.Patrons.Add(patron);
                context.Holds.Add(hold);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                var result = sut.GetCurrentHolds(2319).ToList();
                result.Should().Contain(a => a.Id == 41);
            }
        }

        [Test]
        public void Get_Current_Patron()
        {
        }

        [Test]
        public void Get_CurrentHoldPlaced()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_current_hold_placed")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var hold = new Hold
                {
                    Id = 191,
                    LibraryCard = new LibraryCard {Id = 16},
                    LibraryAsset = new Book
                    {
                        Id = 123,
                        Title = "My Antonia"
                    },
                    HoldPlaced = new DateTime(2000, 05, 11)
                };

                context.Holds.Add(hold);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                var result = sut.GetCurrentHoldPlaced(191);

                var expected = new DateTime(2000, 05, 11).ToString(CultureInfo.InvariantCulture);
                result.Should().Be(expected);
            }
        }

        [Test]
        public void Get_IsCheckedOut()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_checked_out_item")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 322,
                    Title = "Ulysses",
                    NumberOfCopies = 99
                };

                context.Books.Add(book);
                context.SaveChanges();

                var checkout = new Checkout
                {
                    LibraryAsset = book
                };

                context.Checkouts.Add(checkout);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                var result = sut.IsCheckedOut(322);
                result.Should().BeTrue();
            }
        }

        [Test]
        public void Get_Number_Of_Copies()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_number_of_copies")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 2319,
                    Title = "Ulysses",
                    NumberOfCopies = 99
                };

                context.Books.Add(book);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                var result = sut.GetNumberOfCopies(2319);
                result.Should().Be(99);
            }
        }

        [Test]
        public void Mark_Item_Found()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Marks_item_found")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.LibraryAssets.Add(new Book
                {
                    Id = -516,
                    Status = new Status
                    {
                        Id = 2,
                        Name = "Lost"
                    }
                });

                context.Statuses.Add(new Status
                {
                    Id = 1,
                    Name = "Available"
                });

                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.MarkFound(-516);

                var book = context.LibraryAssets.Find(-516);
                book.Status.Name.Should().Be("Available");
            }
        }

        [Test]
        public void Mark_Item_Lost()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Marks_item_lost")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.LibraryAssets.Add(new Book
                {
                    Id = -516,
                    Status = new Status
                    {
                        Id = -123,
                        Name = "Available"
                    }
                });

                context.Statuses.Add(new Status
                {
                    Id = -117,
                    Name = "Lost"
                });
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                sut.MarkLost(-516);

                var book = context.LibraryAssets.Find(-516);
                book.Status.Name.Should().Be("Lost");
            }
        }

        [Test]
        public void Place_Hold()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Places_hold")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.LibraryAssets.Add(new Book
                {
                    Id = -516,
                    Status = new Status
                    {
                        Id = 2,
                        Name = "Available"
                    }
                });

                context.LibraryCards.Add(new LibraryCard
                {
                    Id = 1
                });

                context.Statuses.Add(new Status
                {
                    Id = 1,
                    Name = "On Hold"
                });

                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.PlaceHold(-516, 1);

                var book = context.LibraryAssets.Find(-516);
                book.Status.Name.Should().Be("On Hold");
            }
        }
    }
}