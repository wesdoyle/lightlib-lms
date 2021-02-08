using System;
using System.Collections.Generic;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Web.Models.Catalog {
    public class AssetDetailModel {
        
        public string AssetId { get; set; }
        public string ImageUrl { get; set; }
        public string ItemStatus { get; set; }
        public decimal Cost { get; set; }
        public string CurrentBranchLocation { get; set; }
        
        public string PatronName { get; set; }
        public CheckoutDto LatestCheckout { get; set; }
        public PaginationResult<CheckoutHistoryDto> CheckoutHistory { get; set; }
        public PaginationResult<HoldDto> CurrentHolds { get; set; }
        public List<string> Tags { get; set; }
        
        public BookDto Book { get; set; }
        public DvdDto DVD { get; set; }
        public AudioBookDto AudioBook { get; set; }
        public PeriodicalDto Periodical { get; set; }
        public AudioCdDto AudioCD { get; set; }
        
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