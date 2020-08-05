using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers {
    public class HomeController : LibraryController {
        public IActionResult Index() {
            return View();
        }
    }
}