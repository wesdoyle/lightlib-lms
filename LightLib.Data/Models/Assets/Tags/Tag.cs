using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightLib.Data.Models.Assets.Tags {
    [Table("tags")]
    public class Tag {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public List<AssetTag> AssetTags { get; set; }
    }
}