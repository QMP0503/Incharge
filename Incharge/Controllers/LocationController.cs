using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
