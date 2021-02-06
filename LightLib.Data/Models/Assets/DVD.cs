using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LightLib.Data.Models.Assets {
    public class DVD {
        [Key] public int Id { get; set; }
        [Required] public Guid AssetId { get; set; }
        [Required] public string Title { get; set; }
        [Required] public int Year { get; set; }
        [Required] public string Director { get; set; }
        [Required] public int LengthMinutes { get; set; }
        public string Edition { get; set; }
        public string ISBN { get; set; }
        public string UPC { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string AlternativeTitle { get; set; }
        public List<Tag>  Tags { get; set; }
    }
}