namespace LightLib.Models.DTOs.Assets {
    public sealed class AudioBookDto {
        public int Id { get; set; }
        public string AssetId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ASIN { get; set; }
        public int PublicationYear { get; set; } 
        public int LengthMinutes { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string DeweyIndex { get; set; }
        public string Language { get; set; }
        public string Summary { get; set; }
        public LibraryAssetDto Asset { get; set; }
    }
}