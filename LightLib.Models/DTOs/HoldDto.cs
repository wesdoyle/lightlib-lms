using System;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Models.DTOs {
    public class HoldDto {
        public int Id { get; set; }
        public DateTime HoldPlaced { get; set; }
        public DateTime UpdatedOn { get; set; }
        public LibraryAssetDto LibraryAsset { get; set; }
        public LibraryCardDto LibraryCard { get; set; }
    }
}