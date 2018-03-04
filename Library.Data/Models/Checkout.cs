using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Checkout
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Library Asset")]
        public LibraryAsset LibraryAsset { get; set; }

        [Display(Name = "Library Card")] public LibraryCard LibraryCard { get; set; }

        [Display(Name = "Checked Out Since")] public DateTime Since { get; set; }

        [Display(Name = "Checked Out Until")] public DateTime Until { get; set; }
    }
}