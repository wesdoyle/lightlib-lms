using System.Collections.Generic;
using LightLib.Models.DTOs;

namespace LightLib.Web.Models.Catalog {
    public class AssetIndexListingModel {
        public List<VideoDto> Videos { get; set; }
        public List<BookDto> Books { get; set; }
    }
}