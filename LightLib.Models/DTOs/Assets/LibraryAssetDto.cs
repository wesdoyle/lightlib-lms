using System;

namespace LightLib.Models.DTOs.Assets {
    public class LibraryAssetDto {
        public string Id { get; set; }
        public int Year { get; set; }
        public StatusDto AvailabilityStatus { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public AssetType AssetType { get; set; }
        public virtual LibraryBranchDto Location { get; set; }
        
        public virtual BookDto Book { get; set; }
        public virtual AudioBookDto AudioBook { get; set; }
        public virtual AudioCdDto AudioCD { get; set; }
        public virtual DvdDto DVD { get; set; }
        public virtual PeriodicalDto Periodical { get; set; }

        public AssetType? ViewAssetType {
            get {
                if (Book != null) { return AssetType.Book; }
                if (DVD != null) { return AssetType.DVD; }
                if (AudioBook != null) { return AssetType.AudioBook; }
                if (Periodical != null) { return AssetType.Periodical; }
                if (AudioCD != null) { return AssetType.AudioCD; }
                return null;
            } 
        }

        public string ViewAuthor {
            get {
                if (!String.IsNullOrWhiteSpace(Book?.Author)) { return Book.Author; }
                if (!String.IsNullOrWhiteSpace(DVD?.Director)) { return DVD.Director; }
                if (!String.IsNullOrWhiteSpace(AudioBook?.Author)) { return AudioBook.Author; }
                if (!String.IsNullOrWhiteSpace(Periodical?.Publisher)) { return Periodical.Publisher; }
                if (!String.IsNullOrWhiteSpace(AudioCD?.AssetId)) { return AudioCD.Artist; }
                return "Unknown";
            } 
        }
        
        public string ViewTitle {
            get {
                if (!String.IsNullOrWhiteSpace(Book?.Title)) { return Book.Title; }
                if (!String.IsNullOrWhiteSpace(DVD?.Title)) { return DVD.Title; }
                if (!String.IsNullOrWhiteSpace(AudioBook?.Title)) { return AudioBook.Title; }
                if (!String.IsNullOrWhiteSpace(Periodical?.UniformTitle)) { return Periodical.UniformTitle; }
                if (!String.IsNullOrWhiteSpace(AudioCD?.Title)) { return AudioCD.Title; }
                return "Unknown Title";
            } 
        }
    }
}