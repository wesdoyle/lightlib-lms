using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightLib.Data.Models.Assets {
    [Table("audio_cds")]
    public class AudioCd {
        [Key] public int Id { get; set; }
        [Required] public Guid AssetId { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Artist { get; set; }
        public int PublicationYear { get; set; } 
        public string Label { get; set; }
        public string DeweyIndex { get; set; }
        public string Language { get; set; }
        public string Genre { get; set; }
        public string Summary { get; set; }
        public Asset Asset { get; set; }
    }
}