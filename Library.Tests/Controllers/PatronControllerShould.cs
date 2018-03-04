using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Controllers;
using Library.Models.Patron;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;


namespace Library.Tests.Controllers
{
    [TestFixture]
    public class PatronControllerShould
    {
        [Test]
        public void Return_Patron_Index_View()
        {
            var mockPatronService= new Mock<IPatronService>();
            mockPatronService.Setup(r => r.GetAll()).Returns(GetAllPatrons());
            var controller = new PatronController(mockPatronService.Object);

            var result = controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<PatronIndexModel>();
            viewModel.Subject.Patrons.Count().Should().Be(2);
        }

        [Test]
        public void Return_PatronIndexModel()
        {
            var mockPatronService = new Mock<IPatronService>();
            mockPatronService.Setup(r => r.GetAll()).Returns(GetAllPatrons());
            var controller = new PatronController(mockPatronService.Object);

            var result = controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<PatronIndexModel>();
        }

        private static IEnumerable<Patron> GetAllPatrons()
        {
            return new List<Patron>
            {
                new Patron
                {
                    Id = 888,
                    FirstName = "Abc Def",
                    Address = "3 Commerce St",
                    Telephone = "123"
                },

                new Patron
                {
                    Id = 213,
                    FirstName = "Zxy Def",
                    Address = "2 Commerce St",
                    Telephone = "23421"
                }
            };
        }

        private static Patron GetPatron()
        {
            return new Patron 
            {
                Id = 888,
                FirstName = "Abc Def",
                Address = "3 Commerce St",
                Telephone = "123",
            };
        }
    }
}
