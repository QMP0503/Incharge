using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutoMapper;
using log4net;
using Incharge.Service;

namespace Incharge.Controllers
{
    public class LocationController : Controller
    {
        private readonly IService<LocationVM, Location> _LocationService;
        private readonly IPagingService<PaginatedList<Location>> _pagingService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public LocationController(IService<LocationVM, Location> LocationService, IPagingService<PaginatedList<Location>> pagingService, ILog logger, IMapper mapper)
        {
            _LocationService = LocationService;
            _pagingService = pagingService;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Index(
                                                         string sortOrder,
                                                         string currentFilter,
                                                         string searchString,
                                                         int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : string.Empty;
            ViewData["StatusSortParam"] = sortOrder == "Status_asc" ? "Status_desc" : "Status_asc";
            ViewData["CapacitySortParam"] = sortOrder == "Capacity_asc" ? "Capacity_desc" : "Capacity_asc";
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
        public IActionResult Details(int id) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                return View(_LocationService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddLocation()
        {
            //Check if employee/trainger information is needed on display for when new client account is created
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLocation(LocationVM locationVM)
        {
            try
            {
                _LocationService.AddService(locationVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult EditLocation(int id)
        {
            try
            {
                return View(_LocationService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditClient(LocationVM locationVM)
        {
            try
            {
                _LocationService.UpdateService(locationVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        public IActionResult DeleteLocation(int id)
        {
            try
            {
                return View(_LocationService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }

        [HttpPost, ActionName("DeleteLocation")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLocationConfirm(LocationVM locationVM)
        {
            try
            {
                _LocationService.UpdateService(locationVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        [HttpPost, ActionName("UpdateStatus")]
        public IActionResult UpdateStatus(LocationVM locationVM)//dont need bind if html id is used explicitly
        {
            try
            {
                _LocationService.UpdateService(locationVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
    }
}
