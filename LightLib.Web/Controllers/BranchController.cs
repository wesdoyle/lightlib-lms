using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Interfaces;
using LightLib.Web.Models.Branch;
using Microsoft.AspNetCore.Mvc;

namespace LightLib.Web.Controllers {
    public class BranchController : LibraryController {
        private readonly ILibraryBranchService _branchService;

        public BranchController(ILibraryBranchService branchService) {
            _branchService = branchService;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int perPage = 10) {
            var paginatedBranches = await _branchService.GetPaginated(page, perPage);

            if (paginatedBranches.Results.Any()) {
                foreach (var branch in paginatedBranches.Results) {
                    var branchId = branch.Id;
                    branch.IsOpen = await _branchService.IsBranchOpen(branchId);
                    branch.NumberOfAssets = await _branchService.GetAssetCount(branchId);
                    branch.NumberOfPatrons = await _branchService.GetPatronCount(branchId);
                }

                var model = new BranchIndexModel {
                    PageOfBranches = paginatedBranches 
                };

                return View(model);
            }
            
            var emptyModel = new BranchIndexModel {
                PageOfBranches = new PaginationResult<LibraryBranchDto> {
                    Results = new List<LibraryBranchDto>(),
                    PageNumber = page,
                    PerPage = perPage
                }
            };

            return View(emptyModel);
        }
        
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