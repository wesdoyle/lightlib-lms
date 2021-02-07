namespace LightLib.Models.DTOs.Assets {
    public sealed class BookDto {
        public int Id { get; set; }
        public string AssetId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; } 
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string DeweyIndex { get; set; }
        public string Language { get; set; }
        public string Summary { get; set; }
        public LibraryAssetDto Asset { get; set; }
    }
}