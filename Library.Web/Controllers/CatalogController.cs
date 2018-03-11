using System.Linq;
using Library.Data;
using Library.Web.Models.Catalog;
using Library.Web.Models.CheckoutModels;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ILibraryAssetService _assetsService;
        private readonly ICheckoutService _checkoutsService;

        public CatalogController(ILibraryAssetService assetsService, ICheckoutService checkoutsService)
        {
            _assetsService = assetsService;
            _checkoutsService = checkoutsService;
        }

        public IActionResult Index()
        {
            var assetModels = _assetsService.GetAll();

            var listingResult = assetModels
                .Select(a => new AssetIndexListingModel
                {
                    Id = a.Id,
                    ImageUrl = a.ImageUrl,
                    AuthorOrDirector = _assetsService.GetAuthorOrDirector(a.Id),
                    Dewey = _assetsService.GetDeweyIndex(a.Id),
                    CopiesAvailable = _checkoutsService.GetNumberOfCopies(a.Id), // Remove
                    Title = _assetsService.GetTitle(a.Id),
                    Type = _assetsService.GetType(a.Id),
                    NumberOfCopies = _checkoutsService.GetNumberOfCopies(a.Id)
                }).ToList();

            var model = new AssetIndexModel
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assetsService.Get(id);

            var currentHolds = _checkoutsService.GetCurrentHolds(id).Select(a => new AssetHoldModel
            {
                HoldPlaced = _checkoutsService.GetCurrentHoldPlaced(a.Id),
                PatronName = _checkoutsService.GetCurrentHoldPatron(a.Id)
            });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Type = _assetsService.GetType(id),
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assetsService.GetAuthorOrDirector(id),
                CurrentLocation = _assetsService.GetCurrentLocation(id)?.Name,
                Dewey = _assetsService.GetDeweyIndex(id),
                CheckoutHistory = _checkoutsService.GetCheckoutHistory(id),
                CurrentAssociatedLibraryCard = _assetsService.GetLibraryCardByAssetId(id),
                Isbn = _assetsService.GetIsbn(id),
                LatestCheckout = _checkoutsService.GetLatestCheckout(id),
                CurrentHolds = currentHolds,
                PatronName = _checkoutsService.GetCurrentPatron(id)
            };

            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = _assetsService.Get(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkoutsService.IsCheckedOut(id)
            };
            return View(model);
        }

        public IActionResult Hold(int id)
        {
            var asset = _assetsService.Get(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                HoldCount = _checkoutsService.GetCurrentHolds(id).Count()
            };
            return View(model);
        }

        public IActionResult CheckIn(int id)
        {
            _checkoutsService.CheckInItem(id);
            return RedirectToAction("Detail", new {id});
        }

        public IActionResult MarkLost(int id)
        {
            _checkoutsService.MarkLost(id);
            return RedirectToAction("Detail", new {id});
        }

        public IActionResult MarkFound(int id)
        {
            _checkoutsService.MarkFound(id);
            return RedirectToAction("Detail", new {id});
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkoutsService.CheckoutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new {id = assetId});
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkoutsService.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new {id = assetId});
        }
    }
}