using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LightLib.Data.Models.Assets.Tags;

namespace LightLib.Data.Models.Assets {
    
    [Table("assets")]
    public class Asset {
        [Key] public Guid Id { get; set; }
        public AvailabilityStatus AvailabilityStatus { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public LibraryBranch Location { get; set; }
        public List<AssetTag> AssetTags { get; set; }
    }
}