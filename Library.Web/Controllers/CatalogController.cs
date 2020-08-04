using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Library.Service.Interfaces;
using Library.Web.Models.Catalog;
using Library.Web.Models.CheckoutModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class CatalogController : Controller {
        
        private readonly ILibraryAssetService _assetsService;
        private readonly ICheckoutService _checkoutsService;

        public CatalogController(
            ILibraryAssetService assetsService, 
            ICheckoutService checkoutsService) {
            _assetsService = assetsService;
            _checkoutsService = checkoutsService;
        }

        public async Task<IActionResult> Index([FromQuery] int page, [FromQuery] int perPage) {
            
            var paginationServiceResult = await _assetsService.GetAll(page, perPage);

            if (paginationServiceResult.Error != null) {
                var error = paginationServiceResult.Error;
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    error.Message);
            }

            if (paginationServiceResult.Data != null
                && paginationServiceResult.Data.Results.Any()) {

                var assetIndexListingModels = new List<AssetIndexListingModel>();

                foreach (var asset in paginationServiceResult.Data.Results) {
                    var assetId = asset.Id;
                    var authorOrDirector = await _assetsService.GetAuthorOrDirector(assetId);


                    if (asset.AssetType == "Book") {
                        var deweyIndex = await _assetsService.GetDeweyIndex(assetId);
                    }
                }
                
                var listingResult = assetModels
                    .Select(a => new AssetIndexListingModel {
                        Id = a.Id,
                        ImageUrl = a.ImageUrl,
                        AuthorOrDirector = authorOrDirector,
                    }).ToList();

                var model = new AssetIndexModel {
                    Assets = listingResult
                };

                return View(model);
            }
            
            var emptyModel = new AssetIndexModel {
                Assets = new PaginationResult<AssetIndexListingModel>()
            };
            
            return View(emptyModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var asset = _assetsService.Get(id);

            // var currentHolds = _checkoutsService.GetCurrentHolds(id).Select(a => new AssetHoldModel
            // {
            //     // HoldPlaced = _checkoutsService.GetCurrentHoldPlaced(a.Id),
            //     // PatronName = _checkoutsService.GetCurrentHoldPatron(a.Id)
            // });

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
                // CheckoutHistory = _checkoutsService.GetCheckoutHistory(id),
                CurrentAssociatedLibraryCard = _assetsService.GetLibraryCardByAssetId(id),
                Isbn = _assetsService.GetIsbn(id),
                // LatestCheckout = _checkoutsService.GetLatestCheckout(id),
                // CurrentHolds = currentHolds,
                // PatronName = _checkoutsService.GetCurrentPatron(id)
            };

            return View(model);
        }

        public async Task<IActionResult> Checkout(int id)
        {
            var asset = _assetsService.Get(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                // IsCheckedOut = _checkoutsService.IsCheckedOut(id)
            };
            return View(model);
        }

        public async Task<IActionResult> Hold(int id)
        {
            var asset = _assetsService.Get(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                // HoldCount = _checkoutsService.GetCurrentHolds(id).Count()
            };
            return View(model);
        }

        public async Task<IActionResult> CheckIn(int id)
        {
            _checkoutsService.CheckInItem(id);
            return RedirectToAction("Detail", new {id});
        }

        public async Task<IActionResult> MarkLost(int id)
        {
            _checkoutsService.MarkLost(id);
            return RedirectToAction("Detail", new {id});
        }

        public async Task<IActionResult> MarkFound(int id)
        {
            _checkoutsService.MarkFound(id);
            return RedirectToAction("Detail", new {id});
        }

        [HttpPost]
        public async Task<IActionResult> PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkoutsService.CheckoutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new {id = assetId});
        }

        [HttpPost]
        public async Task<IActionResult> PlaceHold(int assetId, int libraryCardId)
        {
            _checkoutsService.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new {id = assetId});
        }
    }
}