namespace LightLib.Models.DTOs {
    public sealed class VideoDto {
        public int Id { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public int Year { get; set; } // Just store as an int for BC
        public StatusDto Status { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public LibraryBranchDto Location { get; set; }
    }
}