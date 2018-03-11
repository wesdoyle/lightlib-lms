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
    public class StatusServiceShould
    {
        private static IEnumerable<Status> GetStatuses()
        {
            return new List<Status>
            {
                new Status
                {
                    Id = 1,
                    Name = "Available"
                },

                new Status
                {
                    Id = 2,
                    Name = "Checked Out"
                },

                new Status
                {
                    Id = 3,
                    Name = "Lost"
                }
            };
        }

        private static Mock<DbSet<Status>> BuildMock()
        {
            var statuses = GetStatuses().AsQueryable();
            var mockSet = new Mock<DbSet<Status>>();
            mockSet.As<IQueryable<Status>>().Setup(b => b.Provider).Returns(statuses.Provider);
            mockSet.As<IQueryable<Status>>().Setup(b => b.Expression).Returns(statuses.Expression);
            mockSet.As<IQueryable<Status>>().Setup(b => b.ElementType).Returns(statuses.ElementType);
            mockSet.As<IQueryable<Status>>().Setup(b => b.GetEnumerator()).Returns(statuses.GetEnumerator());
            return mockSet;
        }

        [Test]
        public void Add_New_Status()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.Statuses).Returns(mockSet.Object);
            var sut = new StatusService(mockCtx.Object);

            sut.Add(new Status());

            mockCtx.Verify(s => s.Add(It.IsAny<Status>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Test]
        public void Get_All_Statuses()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Statuses).Returns(mockSet.Object);

            var sut = new StatusService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeOfType(typeof(Status));
            queryResult.Should().HaveCount(3);
            queryResult.Should().Contain(a => a.Name == "Available");
            queryResult.Should().Contain(a => a.Name == "Checked Out");
            queryResult.Should().Contain(a => a.Name == "Lost");
        }

        [Test]
        public void Get_Status_By_Id()
        {
            var mockSet = BuildMock();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Statuses).Returns(mockSet.Object);

            var sut = new StatusService(mockCtx.Object);
            var queryResult = sut.Get(3);

            queryResult.Should().BeEquivalentTo(new Status {Id = 3, Name = "Lost"});
        }
    }
}