using LightLib.Models;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Web.Models.Catalog {
    public class AssetIndexModel {
        public PaginationResult<LibraryAssetDto> PageOfAssets { get; set; }
    }
}