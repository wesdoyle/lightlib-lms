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
        public void CheckIn_Checks_Out_To_Earliest_Hold()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Checks_out_to_earliest_hold")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var book = new Book
                {
                    Id = 155,
                    Status = new Status {Name = "Checked Out"}
                };

                var checkout = new Checkout
                {
                    Id = 2309,
                    LibraryAsset = book,
                    LibraryCard = new LibraryCard {Id = 5}
                };

                context.Checkouts.Add(checkout);

                var libraryCard = new LibraryCard {Id = 682};

                var earliestHold = new Hold
                {
                    Id = 221,
                    LibraryCard = libraryCard,
                    LibraryAsset = book,
                    HoldPlaced = new DateTime(1954, 12, 30)
                };

                var latestHold = new Hold
                {
                    Id = 1423,
                    LibraryCard = new LibraryCard {Id = 12},
                    LibraryAsset = book,
                    HoldPlaced = new DateTime(2018, 2, 14)
                };

                context.Holds.Add(latestHold);
                context.Holds.Add(earliestHold);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.CheckInItem(155);
                context.LibraryAssets.Find(155).Status.Name.Should().Be("Checked Out");
                context.LibraryCards.Find(682).Checkouts.Should().Contain(v => v.LibraryAsset.Id == 155);
            }
        }

        [Test]
        public void Checks_Out_Item()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Checks_Out_Item")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Statuses.Add(new Status
                {
                    Name = "Checked Out"
                });

                context.LibraryCards.Add(new LibraryCard
                {
                    Id = 990
                });

                context.LibraryAssets.Add(new Book
                {
                    Id = -2126,
                    Status = new Status
                    {
                        Id = 2,
                        Name = "Available"
                    }
                });

                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.CheckoutItem(-2126, 990);
                var book = context.LibraryAssets.Find(-2126);
                book.Status.Name.Should().Be("Checked Out");
            }
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
        public void Get_Checkout_By_Id()
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
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_current_patron")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var card = new LibraryCard
                {
                    Id = 3233
                };

                var patron = new Patron
                {
                    FirstName = "Kevin",
                    LastName = "Shields",
                    LibraryCard = card
                };

                context.Patrons.Add(patron);

                var checkout = new Checkout
                {
                    Id = 1999,
                    LibraryAsset = new Video
                    {
                        Id = 9,
                        Title = "Stranger Things 2"
                    },

                    LibraryCard = card
                };

                context.Checkouts.Add(checkout);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new CheckoutService(context);
                var result = sut.GetCurrentPatron(9);
                result.Should().Be("Kevin Shields");
            }
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
        public void Gets_Checkout()
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

        [Test]
        public void Update_History_When_Checked_In()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Updates_history_when_checked_in")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.CheckoutHistories.Add(new CheckoutHistory
                {
                    Id = 2209,
                    LibraryAsset = new Video
                    {
                        Id = 10,
                        Status = new Status
                        {
                            Id = 12,
                            Name = "Checked Out"
                        }
                    },
                    LibraryCard = new LibraryCard
                    {
                        Id = 42898
                    },
                    CheckedIn = null,
                    CheckedOut = DateTime.Now
                });

                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.CheckInItem(10);

                var history = context.CheckoutHistories.Find(2209);
                history.CheckedIn.Should().NotBeNull();
            }
        }

        [Test]
        public void Update_History_When_Marked_Found()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Updates_history_when_marked_found")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.CheckoutHistories.Add(new CheckoutHistory
                {
                    Id = 31,
                    LibraryAsset = new Book
                    {
                        Id = 16,
                        Status = new Status
                        {
                            Id = 12,
                            Name = "Lost"
                        }
                    },
                    CheckedIn = null,
                    CheckedOut = DateTime.Now
                });

                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var service = new CheckoutService(context);
                service.MarkFound(16);

                var history = context.CheckoutHistories.Find(31);
                history.CheckedIn.Should().NotBeNull();
            }
        }
    }
}