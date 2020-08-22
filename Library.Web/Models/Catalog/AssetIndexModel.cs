using Library.Models;
using Library.Models.DTOs;

namespace Library.Web.Models.Catalog {
    public class AssetIndexModel {
        public PaginationResult<LibraryAssetDto> PageOfAssets { get; set; }
    }
}