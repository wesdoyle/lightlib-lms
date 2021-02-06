using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LightLib.Data.Models.Assets {
    public class Periodical {
        [Key] public int Id { get; set; }
        [Required] public Guid AssetId { get; set; }
        [Required] public string UniformTitle { get; set; }
        [Required] public string Publisher { get; set; }
        [Required] public string ISSN { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public List<Tag> Tags { get; set; }
    }
}