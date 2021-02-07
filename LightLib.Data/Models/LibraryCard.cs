using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightLib.Data.Models {
    [Table("library_cards")]
    public class LibraryCard {
        public int Id { get; set; }
        public decimal CurrentFees { get; set; }
        public DateTime Issued { get; set; }
        public virtual IEnumerable<Checkout> Checkouts { get; set; }
    }
}