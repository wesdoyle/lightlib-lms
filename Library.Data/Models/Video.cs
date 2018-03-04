using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Video : LibraryAsset
    {
        [Required] public string Director { get; set; }
    }
}