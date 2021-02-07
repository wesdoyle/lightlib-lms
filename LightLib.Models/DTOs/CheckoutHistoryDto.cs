using System;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Models.DTOs {
    public class CheckoutHistoryDto {
        public int Id { get; set; }
        public LibraryAssetDto LibraryAsset { get; set; }
        public LibraryCardDto LibraryCard { get; set; }
        public DateTime CheckedOut { get; set; }
        public DateTime? CheckedIn { get; set; }
    }
}