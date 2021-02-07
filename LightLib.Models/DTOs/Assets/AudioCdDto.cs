namespace LightLib.Models.DTOs.Assets {
    public sealed class AudioCdDto {
        public int Id { get; set; }
        public string AssetId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int PublicationYear { get; set; } 
        public string Label { get; set; }
        public string DeweyIndex { get; set; }
        public string Language { get; set; }
        public string Genre { get; set; }
        public string Summary { get; set; }
        public LibraryAssetDto Asset { get; set; }
    }
}