using System;
using System.ComponentModel.DataAnnotations.Schema;
using LightLib.Data.Models.Assets;

namespace LightLib.Data.Models {
    [Table("holds")]
    public class Hold {
        public DateTime HoldPlaced { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int Id { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
    }
}