using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public abstract class LibraryAsset
    {
        public int Id { get; set; }

        [Required] public string Title { get; set; }
        [Required] public int Year { get; set; } // Just store as an int for BC
        [Required] public Status Status { get; set; }

        [Required]
        [Display(Name = "Cost of Replacement")]
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public virtual LibraryBranch Location { get; set; }
    }
}