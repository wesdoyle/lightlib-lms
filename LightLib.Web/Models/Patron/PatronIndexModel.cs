using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Web.Models.Patron {
    public class PatronIndexModel {
        public PaginationResult<PatronDto> PageOfPatrons { get; set; }
    }
}