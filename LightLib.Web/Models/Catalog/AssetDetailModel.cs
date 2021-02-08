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
    }
}