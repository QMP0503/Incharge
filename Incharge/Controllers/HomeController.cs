
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Incharge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() //make this a dashboard
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
