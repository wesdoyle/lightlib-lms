using System;

namespace Library.Models.DTOs {
    public class HoldDto {
        public int Id { get; set; }
        public LibraryAssetDto LibraryAsset { get; set; }
        public LibraryCardDto LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}