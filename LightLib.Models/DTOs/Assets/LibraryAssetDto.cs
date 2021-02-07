namespace LightLib.Models.DTOs.Assets {
    public class LibraryAssetDto {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public StatusDto Status { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public AssetType AssetType { get; set; }
        public virtual LibraryBranchDto Location { get; set; }
    }
}