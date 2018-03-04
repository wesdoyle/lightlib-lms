using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Controllers;
using Library.Models.Branch;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Library.Tests.Controllers
{
    [TestFixture]
    public class BranchControllerShould
    {
        [Test]
        public void Return_All_Branches()
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
    }
}
