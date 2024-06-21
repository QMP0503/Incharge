using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class AnalyticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
