using System;

namespace LightLib.Models.DTOs.Assets {
    public sealed class PeriodicalDto {
        public int Id { get; set; }
        public string AssetId { get; set; }
        public string UniformTitle { get; set; }
        public string Publisher { get; set; }
        public string ISSN { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public LibraryAssetDto Asset { get; set; }
    }
}