using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightLib.Data.Models.Assets.Tags {
    [Table("asset_tags")]
    public class AssetTag {
        public Tag Tag { get; set; }
        public Asset Asset { get; set; }
        public int TagId { get; set; }
        public Guid AssetId { get; set; }
    }
}