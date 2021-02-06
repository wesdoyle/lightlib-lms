using System;
using LightLib.Data.Models.Assets;

namespace LightLib.Data.Models {
    public class Hold {
        public int Id { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}