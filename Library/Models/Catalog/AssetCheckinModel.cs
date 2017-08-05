using LibraryData.Models;
using System;

namespace Library.Models.Catalog
{
    public class AssetCheckinModel
    {
        public LibraryCard LibraryCard { get; set; }
        public DateTime DateDue { get; set; }
        public DateTime DateCheckedOut { get; set; }
        public decimal FeesCharged { get; set; }
        public int DaysOverDue { get; set; }
    }
}
