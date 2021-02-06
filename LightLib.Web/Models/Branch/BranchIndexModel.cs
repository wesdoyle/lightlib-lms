using LightLib.Models;

namespace LightLib.Web.Models.Branch {
    public class BranchIndexModel {
        public PaginationResult<BranchDetailModel> PageOfBranches { get; set; }
    }
}