using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NaradX.Web.Models;

namespace NaradX.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Default()
        {
            return View();
        }
    }
}
