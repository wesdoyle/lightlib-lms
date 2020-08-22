using Library.Models;
using Library.Models.DTOs;

namespace Library.Web.Models.Patron {
    public class PatronIndexModel {
        public PaginationResult<PatronDto> PageOfPatrons { get; set; }
    }
}