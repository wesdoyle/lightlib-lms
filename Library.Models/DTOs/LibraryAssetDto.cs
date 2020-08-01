namespace Library.Models.DTOs {
    public class LibraryAssetDto {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; } // Just store as an int for BC
        public StatusDto Status { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public virtual LibraryBranchDto Location { get; set; }

        public static explicit operator VideoDto(LibraryAssetDto libraryAsset) => new VideoDto();
        public static explicit operator BookDto(LibraryAssetDto libraryAsset) => new BookDto();
    }
}