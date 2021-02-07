using System.Collections.Generic;

namespace LightLib.Models {
    public class PaginationResult<T> {
        public List<T> Results { get; set; }
        public int PerPage { get; set; }
        public int PageNumber { get; set; }
        public long TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}