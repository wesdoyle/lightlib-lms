using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Data;
using Library.Data.Models;
using Library.Web.Controllers;
using Library.Web.Models.Branch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Library.Tests.Controllers
{
    [TestFixture]
    public class BranchControllerShould
    {
        [Test]
        public void Return_Branch_Index_View()
        {
            var mockBranchService = new Mock<ILibraryBranchService>();
            mockBranchService.Setup(r => r.GetAll()).Returns(GetAllBranches());
            var controller = new BranchController(mockBranchService.Object);

            var result = controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<BranchIndexModel>();
            viewModel.Subject.Branches.Count().Should().Be(3);
        }

        [Test]
        public void Return_BranchIndexModel()
        {
            var mockBranchService = new Mock<ILibraryBranchService>();
            mockBranchService.Setup(r => r.GetAll()).Returns(GetAllBranches());
            var controller = new BranchController(mockBranchService.Object);

            var result = controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<BranchIndexModel>();
        }

        [Test]
        public void Return_Branch_Detail_View()
        {
            var mockBranchService = new Mock<ILibraryBranchService>();
            mockBranchService.Setup(r => r.Get(888)).Returns(GetBranch());
            var controller = new BranchController(mockBranchService.Object);

            var result = controller.Detail(888);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<BranchDetailModel>();
            viewModel.Subject.Address.Should().Be("3 Commerce St");
            viewModel.Subject.Telephone.Should().Be("123");
        }

        [Test]
        public void Return_BranchDetailModel()
        {
            var mockBranchService = new Mock<ILibraryBranchService>();
            mockBranchService.Setup(r => r.Get(888)).Returns(GetBranch());
            var controller = new BranchController(mockBranchService.Object);
            var result = controller.Detail(888);
            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<BranchDetailModel>();
        }

        private static IEnumerable<LibraryBranch> GetAllBranches()
        {
            return new List<LibraryBranch>()
            {
                new LibraryBranch
                {
                    Id = 123,
                    Name = "Sequoia Branch",
                    Address = "1 Main St"
                },

                new LibraryBranch
                {
                    Id = 431,
                    Name = "Lake Branch",
                    Address = "2 Oak Dr"
                },

                new LibraryBranch
                {
                    Id = 888,
                    Name = "Park Branch",
                    Address = "3 Commerce St"
                }
            };
        }

        private static LibraryBranch GetBranch()
        {
            return new LibraryBranch
            {
                Id = 888,
                Name = "Park Branch",
                Address = "3 Commerce St",
                Telephone = "123",
            };
        }
    }
}
