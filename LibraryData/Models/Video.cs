using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class Video : LibraryAsset
    {
        [Required]
        public string Director { get; set; }
    }
}
