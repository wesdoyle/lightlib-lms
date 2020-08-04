using Library.Models;

namespace Library.Web.Models.Branch {
    public class BranchIndexModel {
        public PaginationResult<BranchDetailModel> Branches { get; set; }
    }
}