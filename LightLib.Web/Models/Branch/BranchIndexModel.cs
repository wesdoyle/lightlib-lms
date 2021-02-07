using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Web.Models.Branch {
    public class BranchIndexModel {
        public PaginationResult<LibraryBranchDto> PageOfBranches { get; set; }
    }
}