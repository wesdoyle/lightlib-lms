using System;
using Library.Service.Models;

namespace Library.Service {
    public abstract class BaseLibraryService {
        protected static PagedServiceResult<T> HandleDatabaseCollectionError<T>(Exception ex) {
            return new PagedServiceResult<T> {
                Data = default,
                Error = new ServiceError {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                }
            };
        }

        protected static ServiceResult<T> HandleDatabaseError<T>(Exception ex) {
            return new ServiceResult<T> {
                Data = default,
                Error = new ServiceError {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                }
            };
        }
    }
}