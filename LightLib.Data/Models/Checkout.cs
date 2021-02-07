using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LightLib.Data.Models.Assets;

namespace LightLib.Data.Models {
    [Table("checkouts")]
    public class Checkout {
        public int Id { get; set; }
        [Required] public Asset Asset { get; set; }
        public LibraryCard LibraryCard { get; set; }
        public DateTime CheckedOutSince { get; set; }
        public DateTime CheckedOutUntil { get; set; }
    }
}