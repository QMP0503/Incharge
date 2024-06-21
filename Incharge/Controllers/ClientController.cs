using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
