using LibraryData;
using LibraryData.Models;
using LibraryService;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Library.Tests
{
    [TestFixture]
    public class CheckoutServiceShould
    {
        [Test]
        public void Add_Checkout()
        {
            var mockSet = new Mock<DbSet<Book>>();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);
            var sut = new CheckoutService(mockCtx.Object);

            sut.Add(new Checkout());

            mockCtx.Verify(s => s.Add(It.IsAny<Checkout>()), Times.Once());
            mockCtx.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}