using System;
using System.Collections.Generic;

namespace LightLib.Models.DTOs {
    public class LibraryCardDto {
        public int Id { get; set; }
        public decimal Fees { get; set; }
        public DateTime Created { get; set; }
        public virtual IEnumerable<CheckoutDto> Checkouts { get; set; }
    }
}