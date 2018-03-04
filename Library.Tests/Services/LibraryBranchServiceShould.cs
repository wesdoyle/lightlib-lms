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
        public void Get_LibraryBranch_By_Id()
        {
            var branches = new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Name = "Clover",
                    Id = 1234
                },

                new LibraryBranch
                {
                    Name = "Downtown",
                    Id = -6
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var branch = sut.Get(-6);

            branch.Name.Should().Be("Downtown");
        }

        [Test]
        public void Get_All_LibraryBranches()
        {
            var branches = new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Name = "A",
                    Id = 332 
                },

                new LibraryBranch
                {
                    Name = "Q",
                    Id = -600
                }
            }.AsQueryable();

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
            queryResult.Should().Contain(a => a.Name == "A");
            queryResult.Should().Contain(a => a.Name == "Q");
        }

        [Test]
        public void Get_Branch_Asset_Count()
        {
            var branches = new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Id = -86,
                    LibraryAssets = new List<LibraryAsset>
                    {
                        new Book
                        {
                            Id = 11
                        },

                        new Video
                        {
                            Id = 234
                        },

                        new Book
                        {
                            Id = 145
                        }
                    }
                },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetAssetCount(branches.First().Id);
            queryResult.Should().Be(3);
        }

        [Test]
        public void Get_Branch_Assets()
        {
            var branches = new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Id = -86,
                    LibraryAssets = new List<LibraryAsset>
                    {
                        new Book
                        {
                            Id = 11
                        },

                        new Video
                        {
                            Id = 234
                        },

                        new Book
                        {
                            Id = -145
                        }
                    }
                },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetAssets(branches.First().Id).ToList();
            queryResult.Count.Should().Be(3);
            queryResult.Should().Contain(q => q.Id == -145);
        }

        [Test]
        public void Get_Branch_Assets_Value()
        {
            var branches = new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Id = -86,
                    LibraryAssets = new List<LibraryAsset>
                    {
                        new Book
                        {
                            Id = 11,
                            Cost = 12.50M
                        },

                        new Video
                        {
                            Id = 234,
                            Cost = 100M
                        },

                        new Book
                        {
                            Id = -145,
                            Cost = 10M
                        }
                    }
                },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<LibraryBranch>>();

            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Provider).Returns(branches.Provider);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.Expression).Returns(branches.Expression);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.ElementType).Returns(branches.ElementType);
            mockSet.As<IQueryable<LibraryBranch>>().Setup(b => b.GetEnumerator()).Returns(branches.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.LibraryBranches).Returns(mockSet.Object);

            var sut = new LibraryBranchService(mockCtx.Object);
            var queryResult = sut.GetAssetsValue(branches.First().Id);
            queryResult.Should().Be(122.50M);
        }

        [Test]
        public void Get_Patron_Count()
        {
            var branches = new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Id = 1,
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
            }.AsQueryable();

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
            var branches = new List<LibraryBranch>
            {
                new LibraryBranch
                {
                    Id = 1,
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
            }.AsQueryable();

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
