using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LightLib.Data.Models.Assets.Tags {
    public class Tag {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public List<AssetTag> AssetTags { get; set; }
    }
}