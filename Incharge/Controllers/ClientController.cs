using Incharge.Service;
using Microsoft.AspNetCore.Mvc;
using Incharge.Models;
using Incharge.Service.IService;
using Incharge.Parameters;
using Incharge.ViewModel;
using log4net;

namespace Incharge.Controllers
{
    [BindProperties]
    public class ClientController : Controller
    {
        readonly IClientService _clientService;
        readonly IPagingService<PaginatedList<Client>> _pagingService;
        readonly ILog _logger;
        public ClientController(IClientService clientService, IPagingService<PaginatedList<Client>> pagingService, ILog logger)
        {
            _clientService = clientService;
            _pagingService = pagingService;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(
                                                 string sortOrder,
                                                 string currentFilter,
                                                 string searchString,
                                                 int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : string.Empty;
            ViewData["LastName"] = sortOrder == "LastName" ? "LastName_desc" : "LastName";

            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber));
        }
        [HttpGet]
        public IActionResult Details(string id) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                var clientInfo = new ClientParam();
                clientInfo.Id = id;
                return View(_clientService.FindClient(clientInfo));
            }catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddClient()
        {
            //Check if employee/trainger information is needed on display for when new client account is created
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddClient([Bind("FirstName, LastName, Phone, Email, Status")] ClientVM clientVM)
        {
            try
            {
                _clientService.AddClient(clientVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

    }
}
