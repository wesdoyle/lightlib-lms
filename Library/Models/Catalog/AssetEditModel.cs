using LibraryData.Models;
using System.Collections.Generic;

namespace Library.Models.Catalog
{
    public class AssetEditModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Dewey { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }

        public LibraryBranch CurrentLocation { get; set; }
        public IEnumerable<Status> AvailableStatuses { get; set; }
        public IEnumerable<LibraryBranch> AvailableBranches { get; set; }
        public LibraryCard CurrentLibraryCard { get; set; }
    }
}
