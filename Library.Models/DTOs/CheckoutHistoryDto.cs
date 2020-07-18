using System;

namespace Library.Models.DTOs {
    public class CheckoutHistoryDto {
        public int Id { get; set; }
        public LibraryAssetDto LibraryAsset { get; set; }
        public LibraryCardDto LibraryCard { get; set; }
        public DateTime CheckedOut { get; set; }
        public DateTime? CheckedIn { get; set; }
    }
}