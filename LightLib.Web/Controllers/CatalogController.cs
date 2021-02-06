using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Interfaces;
using LightLib.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace LightLib.Web.Controllers {
    
    /// <summary>
    /// Handles web-layer requests for the Library's Catalog 
    /// </summary>
    public class CatalogController : LibraryController {
        
        private readonly ILibraryAssetService _assetsService;
        private readonly ICheckoutService _checkoutsService;
        private readonly IHoldService _holdService;

        public CatalogController(
            ILibraryAssetService assetsService, 
            IHoldService holdService,
            ICheckoutService checkoutsService) {
            _assetsService = assetsService;
            _checkoutsService = checkoutsService;
            _holdService = holdService;
        }

        /// <summary>
        /// Returns a paginated view of all Library Assets
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index([FromQuery] int page, [FromQuery] int perPage) {
            var paginationServiceResult = await _assetsService.GetAll(page, perPage);
            
            if (paginationServiceResult.Error != null) {
                return HandleServerError(paginationServiceResult.Error);
            }

            if (paginationServiceResult.Data != null && paginationServiceResult.Data.Results.Any()) {
                var allAssets = paginationServiceResult.Data.Results.ToList();
                var viewModel = new AssetIndexModel {
                    PageOfAssets = new PaginationResult<LibraryAssetDto> {
                        Results = allAssets 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IActionResult> Detail(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects to the Check Out Page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IActionResult> VisitCheckOutPage(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks a Library Asset in.  Automatically Handles Checking out to existing holds
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IActionResult> CheckIn(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark an asset as Lost 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> MarkLost(int id) {
            var lostResult = await _assetsService.MarkLost(id);
            return lostResult.Error != null 
                ? HandleServerError(lostResult.Error) 
                : RedirectToAction("Detail", new {id});
        }

        /// <summary>
        /// Mark an asset as Found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> MarkFound(int id) {
            var foundResult = await _assetsService.MarkFound(id);
            return foundResult.Error != null
                ? HandleServerError(foundResult.Error)
                : RedirectToAction("Detail", new {id});
        }

        /// <summary>
        /// Checks out a Library Asset to a Patron's Card 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="libraryCardId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PlaceCheckout(int assetId, int libraryCardId) {
            var checkoutResult = await _checkoutsService.CheckOutItem(assetId, libraryCardId);
            return checkoutResult.Error != null 
                ? HandleServerError(checkoutResult.Error) 
                : RedirectToAction("Detail", new {id = assetId});
        }

        /// <summary>
        /// Places a Hold on a Library Asset to a Patron's Card 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="libraryCardId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PlaceHold(int assetId, int libraryCardId) {
            var holdResult = await _holdService.PlaceHold(assetId, libraryCardId);
            return holdResult.Error != null 
            ? HandleServerError(holdResult.Error) 
            : RedirectToAction("Detail", new {id = assetId});
        }
    }
}