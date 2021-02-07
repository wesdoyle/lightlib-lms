using System;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Models.DTOs {
    public class CheckoutDto {
        public int Id { get; set; }
        public LibraryAssetDto LibraryAsset { get; set; }
        public LibraryCardDto LibraryCard { get; set; }
        public DateTime CheckedOutSince { get; set; }
        public DateTime CheckedOutUntil { get; set; }
    }
}