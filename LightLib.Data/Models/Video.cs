using System.ComponentModel.DataAnnotations;

namespace LightLib.Data.Models {
    public class Video : LibraryAsset {
        [Required] 
        public string Director { get; set; }
    }
}