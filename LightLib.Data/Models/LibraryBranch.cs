using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LightLib.Data.Models.Assets;

namespace LightLib.Data.Models {
    [Table("library_branches")]
    public class LibraryBranch {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string Telephone { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public virtual IEnumerable<Patron> Patrons { get; set; }
        public virtual IEnumerable<Asset> LibraryAssets { get; set; }
        public string ImageUrl { get; set; }
    }
}