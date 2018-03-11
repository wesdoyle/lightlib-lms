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
    public class LibrarybrancheserviceShould
    {
        private static IEnumerable<LibraryBranch> GetBranches()
        {
            return new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Id = -6,
                    Name = "Hawkins",
                    Patrons = new List<Patron>
                    {
                        new Patron
                        {
                            Id = 11,
                            FirstName = "Jin"
                        },

                        new Patron
                        {
                            Id = 234,
                            FirstName = "Helen"
                        }
                    }
                },

                new LibraryBranch
                {
                    Id = 1,
                    Name = "Downtown",
                    Patrons = new List<Patron>
                    {
                        new Patron
                        {
                            Id = 19,
                            FirstName = "Sam"
                        },

                        new Patron
                        {
                            Id = 28,
                            FirstName = "Mark"
                        }
                    },
                    LibraryAssets = new List<LibraryAsset>
                    {
                        new Book
                        {
                            Id = 1,
                            Title = "CTCI",
                            Cost = 1.25M
                        },
                        new Video
                        {
                            Id = 23,
                            Title = "Stranger Things",
                            Cost = 1.25M
                        }
                    }
                }
            };
        }

        [Test]
        public void Add_New_LibraryBranch_To_Context()
        {
            var mockSet = new Mock<DbSet<LibraryBranch>>();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);
            var sut = new LibraryBranchService(mockCtx.Object);

            sut.Add(new LibraryBranch());

            mockCtx.Verify(s => s.Add(It.IsAny<LibraryBranch>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Test]
        public void Get_All_LibraryBranches()
        {
            var branches = GetBranches().AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeOfType(typeof(LibraryBranch));
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(a => a.Name == "Hawkins");
            queryResult.Should().Contain(a => a.Name == "Downtown");
        }

        [Test]
        public void Get_Branch_Asset_Count()
        {
            var branches = GetBranches().AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetAssetCount(1);
            queryResult.Should().Be(2);
        }

        [Test]
        public void Get_Branch_Assets()
        {
            var branches = GetBranches().AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetAssets(1).ToList();
            queryResult.Count.Should().Be(2);
            queryResult.Should().Contain(asset => asset.Id == 1);
        }

        [Test]
        public void Get_Branch_Assets_Value()
        {
            var branches = GetBranches().AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetAssetsValue(1);
            queryResult.Should().Be(2.50M);
        }

        [Test]
        public void Get_Humanized_Branch_Hours()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("Gets_branch_hours")
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var branch = new LibraryBranch {Id = -190};

                var hours = new List<BranchHours>
                {
                    new BranchHours
                    {
                        Branch = branch,
                        DayOfWeek = 1,
                        OpenTime = 13,
                        CloseTime = 15
                    },

                    new BranchHours
                    {
                        Branch = branch,
                        DayOfWeek = 2,
                        OpenTime = 4,
                        CloseTime = 24
                    }
                };

                context.BranchHours.AddRange(hours);
                context.SaveChanges();
            }

            using (var context = new LibraryDbContext(options))
            {
                var sut = new LibraryBranchService(context);
                var result = sut.GetBranchHours(-190);
                var expected = new List<string>
                {
                    "Monday 13:00 to 15:00",
                    "Tuesday 04:00 to 00:00"
                };

                result.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void Get_LibraryBranch_By_Id()
        {
            var branches = GetBranches().AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var branch = sut.Get(-6);

            branch.Name.Should().Be("Hawkins");
        }

        [Test]
        public void Get_Patron_Count()
        {
            var branches = GetBranches().AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetPatronCount(branches.First().Id);
            queryResult.Should().Be(2);
        }

        [Test]
        public void Get_Patrons_Associated_With_Branch()
        {
            var branches = GetBranches().AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetPatrons(branches.First().Id).ToList();
            queryResult.Count.Should().Be(2);
            queryResult.Should().Contain(c => c.FirstName == "Helen");
            queryResult.Should().Contain(c => c.FirstName == "Jin");
        }
    }
}