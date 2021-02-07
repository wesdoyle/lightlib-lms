using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LightLib.Data.Models.Assets;

namespace LightLib.Data.Models {
    [Table("checkouts")]
    public class Checkout {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Library Asset")]
        public Asset Asset { get; set; }

        [Display(Name = "Library Card")] 
        public LibraryCard LibraryCard { get; set; }

        [Display(Name = "Checked Out Since")] 
        public DateTime Since { get; set; }

        [Display(Name = "Checked Out Until")] 
        public DateTime Until { get; set; }
    }
}