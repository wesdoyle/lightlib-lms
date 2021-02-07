using System;

namespace LightLib.Data.Models.Assets.Tags {
    public class AssetTag {
        public Tag Tag { get; set; }
        public Asset Asset { get; set; }
        public int TagId { get; set; }
        public Guid AssetId { get; set; }
    }
}