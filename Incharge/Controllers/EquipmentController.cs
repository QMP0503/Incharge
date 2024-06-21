using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class EquipmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
