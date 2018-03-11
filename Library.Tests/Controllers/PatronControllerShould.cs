using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Data;
using Library.Data.Models;
using Library.Web.Controllers;
using Library.Web.Models.Patron;
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

        [Test]
        public void Return_Patron_Detail_View()
        {
            var mockPatronService = new Mock<IPatronService>();
            mockPatronService.Setup(r => r.Get(1)).Returns(GetPatron());
            mockPatronService.Setup(r => r.GetCheckouts(1)).Returns(new List<Checkout>{});
            mockPatronService.Setup(r => r.GetCheckoutHistory(1)).Returns(new List<CheckoutHistory>{});
            mockPatronService.Setup(r => r.GetHolds(1)).Returns(new List<Hold>{});
            var sut = new PatronController(mockPatronService.Object);

            var result = sut.Detail(1);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<PatronDetailModel>();
            viewModel.Subject.FirstName.Should().Be("Abc Def");
        }

        [Test]
        public void Return_PatronDetailModel()
        {
            var mockPatronService = new Mock<IPatronService>();
            mockPatronService.Setup(r => r.Get(888)).Returns(GetPatron());
            var controller = new PatronController(mockPatronService.Object);

            var result = controller.Detail(888);

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<PatronDetailModel>();
        }
        
        [Test]
        public void Return_Default_Values_For_Missing_Patron_Details()
        {
            var mockPatronService = new Mock<IPatronService>();
            mockPatronService.Setup(r => r.Get(411)).Returns(GetNoInfoPatron());
            var controller = new PatronController(mockPatronService.Object);

            var result = controller.Detail(411);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<PatronDetailModel>();
            viewModel.Subject.FirstName.Should().Be("No First Name Provided");
            viewModel.Subject.LastName.Should().Be("No Last Name Provided");
            viewModel.Subject.Address.Should().Be("No Address Provided");
            viewModel.Subject.HomeLibrary.Should().Be("No Home Library");
            viewModel.Subject.Telephone.Should().Be("No Telephone Number Provided");
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
                LastName = "Last",
                Telephone = "2134",
                Address = "898 Fox Run",
                LibraryCard = new LibraryCard()
                {
                    Id = -123,
                    Created = new DateTime(2018, 2, 12),
                },
                HomeLibraryBranch = new LibraryBranch
                {
                    Id = 14,
                    Name = "Hawkins",
                }
            };
        }
        
        private static Patron GetNoInfoPatron()
        {
            return new Patron();
        }

        private static Patron GetNamelessPatron()
        {
            return new Patron 
            {
                Id = 888,
                Telephone = "2134",
                Address = "898 Fox Run",
                LibraryCard = new LibraryCard()
                {
                    Id = -123,
                    Created = new DateTime(2018, 2, 12),
                },
                HomeLibraryBranch = new LibraryBranch
                {
                    Id = 14,
                    Name = "Hawkins",
                }
            };
        }
    }
}
