using System.ComponentModel.DataAnnotations;

namespace LightLib.Data.Models {
    
    public abstract class LibraryAsset {
        public int Id { get; set; }

        [Required] 
        public string Title { get; set; }
        
        [Required] 
        public int Year { get; set; }
        
        [Required] 
        public Status Status { get; set; }

        [Required]
        [Display(Name = "Cost of Replacement")]
        public decimal Cost { get; set; }
        
        public string ImageUrl { get; set; }
        
        public string AssetType { get; set; }
        public virtual LibraryBranch Location { get; set; }
    }
}