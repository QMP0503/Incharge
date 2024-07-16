using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using log4net;

namespace Incharge.Controllers
{
    public class GymclassController : Controller
    {
        private readonly IService<GymClassVM, Gymclass> _GymclassService;
        private readonly IDropDownOptions<GymClassVM>  _DropdownOptions;
        private readonly IPagingService<PaginatedList<Gymclass>> _pagingService;
        private readonly IGymclassCalendarService _gymclassCalendarService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public GymclassController(IService<GymClassVM, Gymclass> GymclassService, IPagingService<PaginatedList<Gymclass>> pagingService, ILog logger, IMapper mapper, IGymclassCalendarService gymclassCalendarService, IDropDownOptions<GymClassVM> dropdownOptions)
        {
            _GymclassService = GymclassService;
            _pagingService = pagingService;
            _logger = logger;
            _mapper = mapper;
            _gymclassCalendarService = gymclassCalendarService;
            _DropdownOptions = dropdownOptions;
        }


        [HttpGet] //straight calendar view
        public IActionResult Index()
        {
            var WeekdayList = _gymclassCalendarService.CreateItemList();
            return View(WeekdayList);
        }

        [HttpGet] //another view instead of the calendar
        public IActionResult GymclassList(
                                                         string sortOrder,
                                                         string currentFilter,
                                                         string searchString,
                                                         int? pageNumber,
                                                         int pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : string.Empty;
            ViewData["ClassDateSortParam"] = sortOrder == "ClassDate_desc" ? "ClassDate_asc" : "ClassDate_desc";

            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Details(int id) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                return View(_GymclassService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddGymclass()
        {
            var gymclassVM = _DropdownOptions.DropDownOptions();
            return View(gymclassVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGymclass(GymClassVM gymclassVM)
        {
            try
            {
                _GymclassService.AddService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult EditGymclass(GymClassVM gymClassVM)
        {
            try
            {
                var gymclass = _GymclassService.GetItem(x => x.Id == gymClassVM.Id);
                gymclass.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymclass.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymclass.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                return View(gymclass);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }

        }

        [HttpPost, ActionName("EditGymclass")]
        [ValidateAntiForgeryToken]
        public IActionResult EditGymclassConfirm(GymClassVM gymclassVM)
        {
            try
            {
                _GymclassService.UpdateService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        public IActionResult DeleteGymClass(int id)
        {
            try
            {
                return View(_GymclassService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }

        [HttpPost, ActionName("DeleteGymClass")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGymClassConfirm(GymClassVM gymclassVM)
        {
            try
            {
                _GymclassService.UpdateService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        [HttpPost, ActionName("UpdateStatus")]
        public IActionResult UpdateStatus(GymClassVM gymclassVM)//dont need bind if html id is used explicitly
        {
            try
            {
                _GymclassService.UpdateService(gymclassVM);
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
