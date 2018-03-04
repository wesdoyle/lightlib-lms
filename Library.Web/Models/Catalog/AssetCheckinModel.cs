using System;
using Library.Data.Models;

namespace Library.Web.Models.Catalog
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