using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using log4net;
using MySqlX.XDevAPI;
using Microsoft.AspNetCore.Mvc.Filters;
using Incharge.ViewModels.Calendar;

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


        [HttpGet] //will be the index
        public IActionResult Index(
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
        public IActionResult TrainerSchedule(string filter, DateTime dateSelected)
        {
            try
            {
                var WeekdayList = _gymclassCalendarService.CreateItemList("trainer", filter, dateSelected);
                return View(WeekdayList);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                var Error = new List<WeekdayItem>();
                Error.Add(new WeekdayItem() { Error = ex.Message });
                return View(Error);
            }
        }
        [HttpGet] 
        public IActionResult LocationSchedule(string filter, DateTime dateSelected)
        {
            try
            {
                var WeekdayList = _gymclassCalendarService.CreateItemList("location", filter, dateSelected);
                return View(WeekdayList);
            }catch (Exception ex)
            {
                _logger.Error(ex);
                var Error = new List<WeekdayItem>();
                Error.Add(new WeekdayItem() { Error = ex.Message });
                return View(Error);
            }
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
        public IActionResult AddGymclass(string type)
        {
            var gymclassVM = _DropdownOptions.DropDownOptions();
            gymclassVM.Type = type;
            gymclassVM.Date = DateTime.Now; //setting reasonable default value
            gymclassVM.EndTime = DateTime.Now.AddHours(1);
            return View(gymclassVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGymclass(GymClassVM gymclassVM)
        {
            if (!ModelState.IsValid)
            {
                gymclassVM.Error = "Invalid inputs";
                gymclassVM.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymclassVM.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymclassVM.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                gymclassVM.ClientOptions = _DropdownOptions.DropDownOptions().ClientOptions;
                return View(gymclassVM);
            }
            try
            {
                _GymclassService.AddService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                gymclassVM.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymclassVM.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymclassVM.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                gymclassVM.ClientOptions = _DropdownOptions.DropDownOptions().ClientOptions;
                gymclassVM.Error = ex.Message;
                return View(gymclassVM);
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
            if (!ModelState.IsValid)
            {
                gymclassVM.Error = "Invalid inputs";
                gymclassVM.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymclassVM.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymclassVM.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                gymclassVM.ClientOptions = _DropdownOptions.DropDownOptions().ClientOptions;
                return View(gymclassVM);
            }
            try
            {
                _GymclassService.UpdateService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                gymclassVM.Error = ex.Message;
                gymclassVM.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymclassVM.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymclassVM.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                gymclassVM.ClientOptions = _DropdownOptions.DropDownOptions().ClientOptions;
                return View(gymclassVM);
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
        [HttpPost]
        public IActionResult UpdateStatus(GymClassVM gymclassVM)//accessed in detials page
        {
            try
            {
                _GymclassService.UpdateService(gymclassVM);
                return RedirectToAction("Details", new { id = gymclassVM.Id });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
    }
}
