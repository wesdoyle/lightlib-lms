using System;
using System.ComponentModel.DataAnnotations;

namespace LightLib.Data.Models.Assets {
    
    public class LibraryAsset {
        [Key] public Guid Id { get; set; }
        public AvailabilityStatus AvailabilityStatus { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public LibraryBranch Location { get; set; }
    }
}