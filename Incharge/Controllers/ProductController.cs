using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
