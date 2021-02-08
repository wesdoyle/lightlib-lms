using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs.Assets;
using LightLib.Service.Interfaces;
using LightLib.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace LightLib.Web.Controllers {
    
    public class AssetsController : LibraryController {
        
        private readonly ILibraryAssetService _assetsService;
        private readonly ICheckoutService _checkoutsService;
        private readonly IHoldService _holdService;

        public AssetsController(
            ILibraryAssetService assetsService, 
            IHoldService holdService,
            ICheckoutService checkoutsService) {
            _assetsService = assetsService;
            _checkoutsService = checkoutsService;
            _holdService = holdService;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int perPage = 8) {
            var pageOfAssets = await _assetsService.GetPaginated(page, perPage);
            
            if (pageOfAssets != null && pageOfAssets.Results.Any()) {
                var allAssets = pageOfAssets.Results.ToList();
                var viewModel = new AssetIndexModel {
                    PageOfAssets = new PaginationResult<LibraryAssetDto> {
                        Results = allAssets,
                        PageNumber = page,
                        PerPage = perPage,
                        // TODO
                        HasNextPage = true,
                        HasPreviousPage = true
                    }
                };

                return View(viewModel);
            }
            
            var emptyModel = new AssetIndexModel {
                PageOfAssets = new PaginationResult<LibraryAssetDto>() {
                    Results = new List<LibraryAssetDto>(),
                    PerPage = perPage,
                    PageNumber = page
                }
            };
            
            return View(emptyModel);
        }

        public async Task<IActionResult> Detail(
            string assetId, 
            [FromQuery] int checkoutHistoryPage = 1, 
            [FromQuery] int checkoutHistoryPerPage = 5,
            [FromQuery] int holdsPage = 1,
            [FromQuery] int holdsPerPage = 5) {
            
            var assetGuid = Guid.Parse(assetId);
            var asset = await _assetsService.Get(assetGuid);
            var currentBranch = await _assetsService.GetCurrentLocation(assetGuid);
            var currentPatron = await _checkoutsService.GetCurrentCheckoutPatronForAsset(assetGuid);
            var latestCheckout = await _checkoutsService.GetLatestCheckoutForAsset(assetGuid);
            
            var checkoutHistory = await _checkoutsService.GetCheckoutHistory(
                assetGuid, 
                checkoutHistoryPage, 
                checkoutHistoryPerPage);
            
            var currentHolds = await _holdService.GetCurrentHoldsPaginated(
                assetGuid, 
                holdsPage, 
                holdsPerPage);

            var model = new AssetDetailModel() {
                AssetId = asset.Id,
                ImageUrl = asset.ImageUrl,
                ItemStatus = asset.AvailabilityStatus.Name,
                Cost = asset.Cost,
                CurrentBranchLocation = currentBranch.Name,
                PatronName = currentPatron,
                LatestCheckout = latestCheckout,
                CheckoutHistory = checkoutHistory,
                CurrentHolds = currentHolds,
                
                // TODO: Tags feature
                Tags = new List<string>(),
                
                Book = asset.Book,
                DVD = asset.DVD,
                Periodical = asset.Periodical,
                AudioBook = asset.AudioBook,
                AudioCD = asset.AudioCD,
            };

            return View(model);
        }

        public async Task<IActionResult> VisitCheckOutPage(int id) {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> CheckIn(string assetId) {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> MarkLost(string assetId) {
            var assetGuid = Guid.Parse(assetId);
            await _assetsService.MarkLost(assetGuid);
            return RedirectToAction("Detail", new {assetGuid});
        }

        public async Task<IActionResult> MarkFound(string assetId) {
            var assetGuid = Guid.Parse(assetId);
            await _assetsService.MarkFound(assetGuid);
            return RedirectToAction("Detail", new {assetGuid});
        }

        [HttpPost]
        public async Task<IActionResult> PlaceCheckout(string assetId, int libraryCardId) {
            var assetGuid = Guid.Parse(assetId);
            await _checkoutsService.CheckOutItem(assetGuid, libraryCardId);
            return RedirectToAction("Detail", new {id = assetId});
        }
        
        [HttpPost]
        public async Task<IActionResult> PlaceHold(string assetId, int libraryCardId) {
            var assetGuid = Guid.Parse(assetId);
            await _holdService.PlaceHold(assetGuid, libraryCardId);
            return RedirectToAction("Detail", new {id = assetId});
        }
    }
}