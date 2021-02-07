using System;
using System.Collections.Generic;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Models.DTOs {
    public class LibraryBranchDto {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public string ImageUrl { get; set; }
        public List<PatronDto> Patrons { get; set; }
        public List<LibraryAssetDto> LibraryAssets { get; set; }
        
        public bool IsOpen { get; set; }
        public List<string> HoursOpen { get; set; }
        public int NumberOfPatrons { get; set; }
        public int NumberOfAssets { get; set; }
        public decimal TotalAssetValue { get; set; }
    }
}