using Microsoft.AspNetCore.Mvc;
using Incharge.Models;
using Incharge.Service.IService;
using log4net;
using Incharge.Service.PagingService;
using Incharge.ViewModels;

namespace Incharge.Controllers
{
    [BindProperties]
    public class ClientController : Controller
    {
        readonly IService<ClientVM, Client> _clientService;
        readonly IPagingService<PaginatedList<Client>> _pagingService;
        readonly ILog _logger;
        public ClientController(IService<ClientVM, Client> clientService, IPagingService<PaginatedList<Client>> pagingService, ILog logger)
        {
            _clientService = clientService;
            _pagingService = pagingService;
            _logger = logger;
        }
        [HttpGet] //there will be a button in index to check client in. Design a button/page to do so. Check them in and out.
        public IActionResult Index(
                                                 string sortOrder,
                                                 string currentFilter,
                                                 string searchString,
                                                 int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : string.Empty;
            ViewData["LastNameSortParam"] = sortOrder == "LastName" ? "LastName_desc" : "LastName";

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
        public IActionResult Details(ClientVM clientVM) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                return View(_clientService.GetItem(x => x.Uuid == clientVM.Uuid));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddClient()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddClient(ClientVM clientVM)
        {
            try
            {
                clientVM.Status = "Signed In";
                _clientService.AddService(clientVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult EditClient(string Uuid)
        {
            try
            {
                return View(_clientService.GetItem(x => x.Uuid == Uuid));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditClient(ClientVM clientVM)
        {
            try
            {
                _clientService.UpdateService(clientVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        //public IActionResult DeleteClient(string Uuid)
        //{
        //    try
        //    {
        //        return View(_clientService.GetItem(x => x.Uuid == Uuid));
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.Error(ex);
        //        return NotFound();
        //    }
        //}

        [HttpPost, ActionName("DeleteClient")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteClientConfirm(ClientVM clientVM)
        {
            try
            {
                _clientService.DeleteService(clientVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        [HttpPost, ActionName("UpdateStatus")]
        public IActionResult UpdateStatus(/*[Bind("Uuid, Status")]*/ ClientVM clientVM)//dont need bind if html id is used explicitly
        {
            try
            {
                if(clientVM.Status =="Sign In")
                {
                    clientVM.Status = "Signed In";
                }
                if(clientVM.Status == "Sign Out")
                {
                    clientVM.Status = "Signed Out";
                }
                _clientService.UpdateService(clientVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
    }
}
