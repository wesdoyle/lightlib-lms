using Library.Models;

namespace Library.Web.Models.Branch {
    public class BranchIndexModel {
        public PaginationResult<BranchDetailModel> PageOfBranches { get; set; }
    }
}