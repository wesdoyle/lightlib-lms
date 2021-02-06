using LightLib.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LightLib.Web.Controllers {
    public abstract class LibraryController : Controller {
        internal IActionResult HandleServerError(ServiceError err) {
            var error = err;
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}