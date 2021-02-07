using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LightLib.Data.Models.Assets;

namespace LightLib.Data.Models {
    [Table("checkout_histories")]
    public class CheckoutHistory {
        public int Id { get; set; }

        [Required] 
        public Asset Asset { get; set; }

        [Required] 
        public LibraryCard LibraryCard { get; set; }

        [Required] 
        public DateTime CheckedOut { get; set; }

        public DateTime? CheckedIn { get; set; }
    }
}