using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
