using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutoMapper;
using log4net;
using Incharge.Repository;

namespace Incharge.Controllers
{
    public class LocationController : Controller
    {
        private readonly IService<LocationVM, Location> _LocationService;
        private readonly IPagingService<PaginatedList<Location>> _pagingService;
        private readonly IChecker<LocationVM> _locationChecker;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public LocationController(IService<LocationVM, Location> LocationService, IPagingService<PaginatedList<Location>> pagingService, ILog logger, IMapper mapper, IChecker<LocationVM> locationChecker)
        {
            _LocationService = LocationService;
            _pagingService = pagingService;
            _logger = logger;
            _mapper = mapper;
            _locationChecker = locationChecker;
        }


        [HttpGet]
        public IActionResult Index(
                                                         string sortOrder,
                                                         string currentFilter,
                                                         string searchString,
                                                         int? pageNumber, 
                                                         int pageSize)
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

            _locationChecker.Check();

            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber, pageSize));
        }
        [HttpGet]
        public IActionResult Details(LocationVM locationVM) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                return View(_LocationService.GetItem(x => x.Id == locationVM.Id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddLocation()
        {
            return View(new LocationVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLocation(LocationVM locationVM)
        {
            if(ModelState.IsValid == false)
            {
                locationVM.Error = "Invalid inputs!";
                return View(locationVM);
            }
            try
            {
                _LocationService.AddService(locationVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if(ex.InnerException != null) { locationVM.Error = ex.InnerException.Message; }
                else { locationVM.Error = ex.Message; }
                return View(locationVM);
            }
        }
        public IActionResult EditLocation(LocationVM locationVM)
        {
            try
            {
                var location = _LocationService.GetItem(x => x.Id == locationVM.Id);
                location.Error = locationVM.Error;
                return View(location);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }

        }

        [HttpPost, ActionName("EditLocation")]
        [ValidateAntiForgeryToken]
        public IActionResult EditLocationConfirm(LocationVM locationVM)
        {
            if(ModelState.IsValid == false)
            {
                locationVM.Error = "Invalid inputs!";
                return View(locationVM);
            }
            try
            {
                _LocationService.UpdateService(locationVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { locationVM.Error = ex.InnerException.Message; }
                else { locationVM.Error = ex.Message; }
                return View(locationVM);
            }
        }

        [HttpPost, ActionName("DeleteLocation")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLocation(LocationVM locationVM)
        {
            try
            {
                _LocationService.DeleteService(locationVM);
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
