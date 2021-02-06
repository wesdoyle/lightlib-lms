namespace LightLib.Models.DTOs {
    public sealed class BookDto {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string DeweyIndex { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public StatusDto Status { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public LibraryBranchDto Location { get; set; }
    }
}