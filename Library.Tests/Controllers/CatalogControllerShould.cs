using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Data;
using Library.Data.Models;
using Library.Web.Controllers;
using Library.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Library.Tests.Controllers
{
    [TestFixture]
    public class CatalogControllerShould
    {
        [Test]
        public void Return_Catalog_Index_View()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();

            mockLibraryAssetService.Setup(r => r.GetAll()).Returns(GetAllAssets());

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Index();
            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetIndexModel>();
            viewModel.Subject.Assets.Count().Should().Be(2);
        }

        [Test]
        public void Return_AssetIndexListingModel()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();

            mockLibraryAssetService.Setup(r => r.GetAll()).Returns(GetAllAssets());

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Index();
            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<AssetIndexModel>();
        }

        [Test]
        public void Return_Branch_Detail_View()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();

            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(24)).Returns(new LibraryBranch
            {
                Name = "Main St. Library"
            });

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Detail(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetDetailModel>();

            viewModel.Subject.Title.Should().Be("Orlando");
        }

        [Test]
        public void Return_BranchDetailModel()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();

            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(24)).Returns(new LibraryBranch
            {
                Name = "Main St. Library"
            });
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);
            var result = controller.Detail(24);
            var viewResult = result.Should().BeOfType<ViewResult>();

            viewResult.Subject.Model.Should().BeOfType<AssetDetailModel>();
        }

        [Test]
        public void Return_CheckIn_View()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();

            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(24)).Returns(new LibraryBranch
            {
                Name = "Main St. Library"
            });

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Detail(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetDetailModel>();

            viewModel.Subject.Title.Should().Be("Orlando");
        }

        [Test]
        public void Call_CheckInItem_In_Service_When_CheckIn_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();
            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.CheckIn(24);

            mockCheckoutService.Verify(s => s.CheckInItem(24), Times.Once());
        }

        [Test]
        public void Return_DetailView_When_CheckIn_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();
            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.CheckIn(24);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Call_MarkLost_In_Service_When_MarkLost_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();
            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.MarkLost(24);

            mockCheckoutService.Verify(s => s.MarkLost(24), Times.Once());
        }

        [Test]
        public void Return_DetailView_When_MarkLost_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();
            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.MarkLost(24);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Call_MarkFound_In_Service_When_MarkFound_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();
            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.MarkFound(24);

            mockCheckoutService.Verify(s => s.MarkFound(24), Times.Once());
        }

        [Test]
        public void Return_DetailView_When_MarkFound_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();
            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = sut.MarkFound(24);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Return_CheckOut_View()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();

            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(24)).Returns(new LibraryBranch
            {
                Name = "Main St. Library"
            });

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Detail(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetDetailModel>();

            viewModel.Subject.Title.Should().Be("Orlando");
        }

        [Test]
        public void Return_CheckOutModel()
        {
            var mockLibraryAssetService = new Mock<ILibraryAssetService>();
            var mockCheckoutService = new Mock<ICheckoutService>();

            mockLibraryAssetService.Setup(r => r.Get(24)).Returns(GetAsset());
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(24)).Returns(new LibraryBranch
            {
                Name = "Main St. Library"
            });
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);
            var result = controller.Detail(24);
            var viewResult = result.Should().BeOfType<ViewResult>();

            viewResult.Subject.Model.Should().BeOfType<AssetDetailModel>();
        }

        private static IEnumerable<LibraryAsset> GetAllAssets()
        {
            return new List<LibraryAsset>
            {
                new Book
                {
                    Title = "Orlando",
                    Author = "Virginia Woolf",
                    Status = new Status()
                    {
                        Name = "Checked In",
                        Id = 1,
                    },
                },

                new Video
                {
                    Title = "Happy People",
                    Director = "Werner Herzog",
                    ImageUrl = "images/sample.jpg",
                    Status = new Status()
                    {
                        Name = "Lost",
                        Id = 3,
                    },
                }
            };
        }

        private static Book GetAsset()
        {
            return new Book
            {
                Title = "Orlando",
                Author = "Virginia Woolf",
                Status = new Status()
                {
                    Name = "Checked In",
                    Id = 1,
                },
                Location = new LibraryBranch()
                {
                    Id = -99,
                    Name = "Hawkins"
                }
            };
        }
    }
}
