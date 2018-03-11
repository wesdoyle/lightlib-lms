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
    public class LibraryAssetServiceShould
    {
        private static IEnumerable<LibraryAsset> GetAssets()
        {
            return new List<LibraryAsset>
            {
                new Book
                {
                    Id = 1223,
                    Title = "Infinite Jest",
                    Author = "DFW",
                    DeweyIndex = "WAL111"
                },

                new Book
                {
                    Id = -903,
                    Title = "Beyond the Bedroom Wall",
                    Location = new LibraryBranch {Id = 200},
                    ISBN = "FOO"
                },

                new Video
                {
                    Id = 234,
                    Title = "The Matrix",
                    Director = "WB"
                }
            };
        }

        private Mock<DbSet<LibraryAsset>> BuildMock()
        {
            var assets = GetAssets().AsQueryable();
            var mockSet = new Mock<DbSet<LibraryAsset>>();
            mockSet.As<IQueryable<LibraryAsset>>().Setup(b => b.Provider).Returns(assets.Provider);
            mockSet.As<IQueryable<LibraryAsset>>().Setup(b => b.Expression).Returns(assets.Expression);
            mockSet.As<IQueryable<LibraryAsset>>().Setup(b => b.ElementType).Returns(assets.ElementType);
            mockSet.As<IQueryable<LibraryAsset>>().Setup(b => b.GetEnumerator()).Returns(assets.GetEnumerator());
            return mockSet;
        }

        [Test]
        public void Add_New_Asset()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Add_asset_writes_to_database")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var service = new LibraryAssetService(context);

                service.Add(new Book
                {
                    Id = -27
                });

                Assert.AreEqual(-27, context.LibraryAssets.Single().Id);
            }
        }

        [Test]
        public void Get_All_Assets()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeAssignableTo(typeof(LibraryAsset));
            queryResult.Should().HaveCount(3);
            queryResult.Should().Contain(a => a.Title == "Infinite Jest");
        }

        [Test]
        public void Get_Asset_By_Id()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.Get(234);
            var expected = new Video
            {
                Id = 234,
                Title = "The Matrix",
                Director = "WB"
            };

            queryResult.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Get_Asset_Title()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetTitle(234);
            queryResult.Should().Be("The Matrix");
        }

        [Test]
        public void Get_Asset_Type_Given_Book()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetType(-903);
            queryResult.Should().Be("Book");
        }

        [Test]
        public void Get_Asset_Type_Given_Video()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetType(234);
            queryResult.Should().Be("Video");
        }

        [Test]
        public void Get_Author_Given_Book()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetAuthorOrDirector(1223);
            queryResult.Should().Be("DFW");
        }

        [Test]
        public void Get_Current_Location()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetCurrentLocation(-903);
            queryResult.Should().BeEquivalentTo(new LibraryBranch {Id = 200});
        }

        [Test]
        public void Get_Dewey_Index_For_Book()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetDeweyIndex(1223);
            queryResult.Should().Be("WAL111");
        }

        [Test]
        public void Get_Dewey_Index_NA_For_Video()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);
            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetDeweyIndex(234);
            queryResult.Should().Be("N/A");
        }

        [Test]
        public void Get_Director_Given_Video()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetAuthorOrDirector(234);
            queryResult.Should().Be("WB");
        }

        [Test]
        public void Get_Isbn_For_Book()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetIsbn(-903);
            queryResult.Should().Be("FOO");
        }

        [Test]
        public void Get_Library_Card_By_Asset_Id()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_library_card_for_asset")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var card = new LibraryCard
                {
                    Id = 16,
                    Created = new DateTime(1999, 1, 1),
                    Fees = 0M
                };

                var checkout = new Checkout
                {
                    Id = 87,
                    LibraryAsset = new Book {Id = 300},
                    LibraryCard = card
                };

                context.Checkouts.Add(checkout);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new LibraryAssetService(context);
                var result = sut.GetLibraryCardByAssetId(300);
                result.Should()
                    .BeEquivalentTo(new LibraryCard {Id = 16, Created = new DateTime(1999, 1, 1), Fees = 0M});
            }
        }

        [Test]
        public void Return_NA_ISBN_For_NonBook_Asset()
        {
            var mockSet = BuildMock();

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryAssets).Returns(mockSet.Object);

            var sut = new LibraryAssetService(mockCtx.Object);
            var queryResult = sut.GetIsbn(234);
            queryResult.Should().Be("N/A");
        }
    }
}