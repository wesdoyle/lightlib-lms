using Library.Models;

namespace Library.Web.Models.Catalog {
    public class AssetIndexModel {
        public PaginationResult<AssetIndexListingModel> PageOfAssets { get; set; }
    }
}