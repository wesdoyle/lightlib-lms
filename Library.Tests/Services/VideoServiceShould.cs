using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class VideoServiceShould
    {
        [Test]
        public void Get_All_Videos()
        {
            var videos = new List<Video>
            {
                new Video 
                {
                    Id = 1234
                },

                new Video 
                {
                    Id = -6
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Video>>();
            mockSet.As<IQueryable<Video>>().Setup(b => b.Provider).Returns(videos.Provider);
            mockSet.As<IQueryable<Video>>().Setup(b => b.Expression).Returns(videos.Expression);
            mockSet.As<IQueryable<Video>>().Setup(b => b.ElementType).Returns(videos.ElementType);
            mockSet.As<IQueryable<Video>>().Setup(b => b.GetEnumerator()).Returns(videos.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Videos).Returns(mockSet.Object);

            var sut = new VideoService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            queryResult.Should().AllBeOfType(typeof(Video));
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(a => a.Id == -6);
            queryResult.Should().Contain(a => a.Id == 1234);
        }

        [Test]
        public void Get_Video_By_Id()
        {
            var videos = new List<Video>
            {
                new Video 
                {
                    Title = "Def",
                    Id = 324 
                },

                new Video 
                {
                    Title = "Abc",
                    Id = -1
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Video>>();
            mockSet.As<IQueryable<Video>>().Setup(b => b.Provider).Returns(videos.Provider);
            mockSet.As<IQueryable<Video>>().Setup(b => b.Expression).Returns(videos.Expression);
            mockSet.As<IQueryable<Video>>().Setup(b => b.ElementType).Returns(videos.ElementType);
            mockSet.As<IQueryable<Video>>().Setup(b => b.GetEnumerator()).Returns(videos.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Videos).Returns(mockSet.Object);

            var sut = new VideoService(mockCtx.Object);
            var video = sut.Get(-1);

            video.Title.Should().Be("Abc");
        }

        [Test]
        public void Get_Video_By_Director_Partial_Match()
        {
            var videos = new List<Video>
            {
                new Video
                {
                    Director = "Jim Jarmusch",
                    Title = "Ghost Dog"
                },

                new Video
                {
                    Director = "Krzysztof Kieślowski",
                    Title = "Trois couleurs: Bleu"
                },

                new Video
                {
                    Director = "Krzysztof Kieślowski",
                    Title = "Trois couleurs: Rouge"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Video>>();

            mockSet.As<IQueryable<Video>>().Setup(b => b.Provider).Returns(videos.Provider);
            mockSet.As<IQueryable<Video>>().Setup(b => b.Expression).Returns(videos.Expression);
            mockSet.As<IQueryable<Video>>().Setup(b => b.ElementType).Returns(videos.ElementType);
            mockSet.As<IQueryable<Video>>().Setup(b => b.GetEnumerator()).Returns(videos.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Videos).Returns(mockSet.Object);

            var sut = new VideoService(mockCtx.Object);
            var queryResult = sut.GetByDirector("Kieślowski").ToList();

            queryResult.Should().AllBeOfType(typeof(Video));
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(b => b.Title == "Trois couleurs: Rouge");
            queryResult.Should().Contain(b => b.Title == "Trois couleurs: Bleu");
        }

        [Test]
        public void Add_New_Video_To_Context()
        {
            var mockSet = new Mock<DbSet<Video>>();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.Videos).Returns(mockSet.Object);
            var sut = new VideoService(mockCtx.Object);

            sut.Add(new Video());

            mockCtx.Verify(s => s.Add(It.IsAny<Video>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}
