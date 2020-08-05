using System.Collections.Generic;
using Library.Models.DTOs;

namespace Library.Web.Models.Catalog {
    public class AssetIndexListingModel {
        public List<VideoDto> Videos { get; set; }
        public List<BookDto> Books { get; set; }
    }
}