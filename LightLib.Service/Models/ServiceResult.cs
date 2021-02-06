using LightLib.Models;

namespace LightLib.Service.Models {
    /// <summary>
    /// 
    /// </summary>
    public class ErrorableServiceResult {
        public ServiceError Error { get; set; }
    }
    
    /// <summary>
    /// Serializes data for all service-level responses
    /// that use non-paginated data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> : ErrorableServiceResult {
        public T Data { get; set; }
    }
    
    /// <summary>
    /// Serializes data for all service-level responses
    /// that use paginated data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedServiceResult<T> : ErrorableServiceResult {
        public PaginationResult<T> Data { get; set; }
    }
}