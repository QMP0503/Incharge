using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class GymClassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
