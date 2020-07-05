using Library.Models;

namespace Library.Service.Models {
    /// <summary>
    /// Serializes data for all service-level responses
    /// that use non-paginated data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> {
        public T Data { get; set; }
        public ServiceError Error { get; set; }
    }
    
    /// <summary>
    /// Serializes data for all service-level responses
    /// that use paginated data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedServiceResult<T> {
        public PaginationResult<T> Data { get; set; }
        public ServiceError Error { get; set; }
    }
}