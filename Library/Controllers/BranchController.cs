using Library.Models.Branch;
using LibraryData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Library.Controllers
{
    public class BranchController : Controller
    {
        private ILibraryBranch _branch;

        // create a constructor takes branchservice
        public BranchController(ILibraryBranch branch)
        {
            // save branchService param off into a private field 
            // to have access in the rest of the controller
            _branch = branch;
        }

        public IActionResult Index()
        {
            var branchModels = _branch.GetAll()
                .Select(br => new BranchDetailModel
                {
                    Id = br.Id,
                    BranchName = br.Name,
                    NumberOfAssets = _branch.GetAssetCount(br.LibraryAssets),
                    NumberOfPatrons = _branch.GetPatronCount(br.Patrons),
                    IsOpen = _branch.IsBranchOpen(br.Id)
                }).ToList();

            var model = new BranchIndexModel()
            {
                Branches = branchModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branch = _branch.Get(id);
            var model = new BranchDetailModel
            {
                BranchName = branch.Name,
                Description = branch.Description,
                Address = branch.Address,
                Telephone = branch.Telephone,
                BranchOpenedDate = branch.OpenDate.ToString("yyyy-MM-dd"),
                NumberOfPatrons = _branch.GetPatronCount(branch.Patrons),
                NumberOfAssets = _branch.GetAssetCount(branch.LibraryAssets),
                TotalAssetValue = _branch.GetAssetsValue(id),
                ImageUrl = branch.ImageUrl,
                HoursOpen = _branch.GetBranchHours(id)
            };

            return View(model);
        }
    }
}
