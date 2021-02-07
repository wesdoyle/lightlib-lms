using System;

namespace LightLib.Models.Exceptions {
    public class LibraryServiceException : Exception {
        public Reason Reason { get; set; }
        
        public LibraryServiceException(Reason reason) {
            Reason = reason;
        }
    }

    public enum Reason {
        NotFound,
        NotAuthorized,
        UncaughtError
    }
}