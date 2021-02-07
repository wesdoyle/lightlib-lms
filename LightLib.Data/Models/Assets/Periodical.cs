using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightLib.Data.Models.Assets {
    [Table("periodicals")]
    public class Periodical {
        [Key] public int Id { get; set; }
        [Required] public Guid AssetId { get; set; }
        [Required] public string UniformTitle { get; set; }
        [Required] public string Publisher { get; set; }
        [Required] public string ISSN { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public Asset Asset { get; set; }
    }
}