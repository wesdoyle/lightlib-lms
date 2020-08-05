using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Library.Service.Interfaces;
using Library.Web.Models.Branch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers {
    /// <summary>
    /// Handles requests for Library Branch resources
    /// </summary>
    public class BranchController : LibraryController {
        private readonly ILibraryBranchService _branchService;

        public BranchController(ILibraryBranchService branchService) {
            _branchService = branchService;
        }

        /// <summary>
        /// Fetch a LibraryBranchIndex model, which contains a paginated collection
        /// of Library Branch information
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index([FromQuery] int page, [FromQuery] int perPage) {
            var paginationServiceResult = await _branchService.GetAll(page, perPage);

            if (paginationServiceResult.Error != null) {
                return HandleServerError(paginationServiceResult.Error);
            }

            if (paginationServiceResult.Data != null 
            && paginationServiceResult.Data.Results.Any()) {
                
                foreach (var branch in paginationServiceResult.Data.Results) {
                    var branchId = branch.Id;
                    
                    var branchOpenResult = await _branchService.IsBranchOpen(branchId);
                    branch.IsOpen = branchOpenResult.Data;

                    var assetCountResult = await _branchService.GetAssetCount(branchId);
                    branch.NumberOfAssets = assetCountResult.Data;

                    var patronCount = await _branchService.GetPatronCount(branchId);
                    branch.NumberOfPatrons = patronCount.Data;
                }

                var branchModels = new PaginationResult<BranchDetailModel>();

                var model = new BranchIndexModel {
                    PageOfBranches = branchModels
                };

                return View(model);
            }
            
            var emptyModel = new BranchIndexModel {
                PageOfBranches = new PaginationResult<BranchDetailModel>()
            };

            return View(emptyModel);
        }
        
        /// <summary>
        /// Fetch a LibraryBranchDetailModel, which contains data about a particular
        /// Library Branch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(int id) {

            var serviceResult = await _branchService.Get(id);

            if (serviceResult.Error != null) {
                // Log the error and stack trace
                // Branch if running in debug mode and show detailed error in view
                var error = serviceResult.Error;
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    error.Message);
            }

            if (serviceResult.Data == null) {
                return NotFound($"The Library Branch with ID {id} was not found");
            }

            var patronCountResult = await _branchService.GetPatronCount(id);
            var patronCount = patronCountResult.Data;
            
            var assetsCountResult = await _branchService.GetAssetCount(id);
            var assetsCount = assetsCountResult.Data;
            
            var assetsValueResult = await _branchService.GetAssetsValue(id);
            var assetsValue = assetsValueResult.Data;

            var branchHoursResult = await _branchService.GetBranchHours(id);
            var branchHours = branchHoursResult.Data ?? new List<string>();

            var model = new BranchDetailModel {
                BranchName = serviceResult.Data.Name,
                Description = serviceResult.Data.Description,
                Address = serviceResult.Data.Address,
                Telephone = serviceResult.Data.Telephone,
                BranchOpenedDate = serviceResult.Data.OpenDate.ToString("yyyy-MM-dd"),
                NumberOfPatrons = patronCount,
                NumberOfAssets = assetsCount,
                TotalAssetValue = assetsValue,
                ImageUrl = serviceResult.Data.ImageUrl,
                HoursOpen = branchHours
            };

            return View(model);

        }
    }
}