namespace Library.Models.DTOs {
    public class BookDto {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string DeweyIndex { get; set; }
        public string Title { get; set; }
        public int Year { get; set; } // Just store as an int for BC
        public StatusDto Status { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public virtual LibraryBranchDto Location { get; set; }
    }
}