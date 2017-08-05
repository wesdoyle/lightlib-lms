using LibraryData.Models;
using System;

namespace Library.Models.Catalog
{
    public class AssetCheckoutModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public LibraryCard LibraryCard { get; set; }
        public DateTime DateDue { get; set; }
        public DateTime DateCheckedOut { get; set; }
    }
}
