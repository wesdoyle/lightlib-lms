using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Service.Interfaces;
using LightLib.Web.Models.Branch;
using Microsoft.AspNetCore.Mvc;

namespace LightLib.Web.Controllers {
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

            if (paginationServiceResult.Results.Any()) {
                
                foreach (var branch in paginationServiceResult.Results) {
                    var branchId = branch.Id;
                    branch.IsOpen = await _branchService.IsBranchOpen(branchId);
                    branch.NumberOfAssets = await _branchService.GetAssetCount(branchId);
                    branch.NumberOfPatrons = await _branchService.GetPatronCount(branchId);
                }

                // TODO
                var branchModels = new PaginationResult<BranchDetailModel>();

                var model = new BranchIndexModel {
                    PageOfBranches = branchModels
                };

                return View(model);
            }
            
            var emptyModel = new BranchIndexModel {
                PageOfBranches = new PaginationResult<BranchDetailModel> {
                    Results = new List<BranchDetailModel>(),
                    PageNumber = page,
                    PerPage = perPage
                }
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

            var patronCount = await _branchService.GetPatronCount(id);
            var assetsCount = await _branchService.GetAssetCount(id);
            var assetsValue = await _branchService.GetAssetsValue(id);
            var branchHoursResult = await _branchService.GetBranchHours(id);
            var branchHours = branchHoursResult ?? new List<string>();

            var model = new BranchDetailModel {
                BranchName = serviceResult.Name,
                Description = serviceResult.Description,
                Address = serviceResult.Address,
                Telephone = serviceResult.Telephone,
                BranchOpenedDate = serviceResult.OpenDate.ToString("yyyy-MM-dd"),
                NumberOfPatrons = patronCount,
                NumberOfAssets = assetsCount,
                TotalAssetValue = assetsValue,
                ImageUrl = serviceResult.ImageUrl,
                HoursOpen = branchHours
            };

            return View(model);
        }
    }
}