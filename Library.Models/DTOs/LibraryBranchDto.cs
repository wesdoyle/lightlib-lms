using System;
using System.Collections.Generic;
using Library.Data.Models;

namespace Library.Models.DTOs {
    public class LibraryBranchDto {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public string ImageUrl { get; set; }
        public List<Patron> Patrons { get; set; }
        public List<LibraryAsset> LibraryAssets { get; set; }
    }
}