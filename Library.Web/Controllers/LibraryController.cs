using Library.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers {
    public abstract class LibraryController : Controller {
        /// <summary>
        /// Handles 500 errors thrown in Controllers 
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        internal IActionResult HandleServerError(ServiceError err) {
            var error = err;
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}