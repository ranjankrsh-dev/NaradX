using Microsoft.AspNetCore.Mvc;

namespace NaradX.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public UserController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Default()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
