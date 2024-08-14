using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using log4net;
using Incharge.ViewModels.Calendar;
using Microsoft.AspNetCore.Authorization;


namespace Incharge.Controllers
{
    [Authorize]
    public class GymclassController : Controller
    {
        private readonly IService<GymClassVM, Gymclass> _GymclassService;
        private readonly IDropDownOptions<GymClassVM> _DropdownOptions;
        private readonly IPagingService<PaginatedList<Gymclass>> _pagingService;
        private readonly IGymclassCalendarService _gymclassCalendarService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;
        private readonly IChecker _checker;

        public GymclassController(IChecker checker, IService<GymClassVM, Gymclass> GymclassService, IPagingService<PaginatedList<Gymclass>> pagingService, ILog logger, IMapper mapper, IGymclassCalendarService gymclassCalendarService, IDropDownOptions<GymClassVM> dropdownOptions)
        {
            _GymclassService = GymclassService;
            _pagingService = pagingService;
            _logger = logger;
            _mapper = mapper;
            _gymclassCalendarService = gymclassCalendarService;
            _DropdownOptions = dropdownOptions;
            _checker = checker;
        }


        [HttpGet] //will be the index
        [Route("/GymClass")]
        public IActionResult Index(
                                                         string sortOrder,
                                                         string currentFilter,
                                                         string searchString,
                                                         int? pageNumber,
                                                         int pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "Name_desc" ? "Name_asc" : "Name_desc";
            ViewData["ClassDateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "ClassDate_asc" : string.Empty;


            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            _checker.LocationCheck();
            _checker.EquipmentCheck();
            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult TrainerSchedule(string filter, DateTime dateSelected)
        {
            try
            {
                var WeekdayList = _gymclassCalendarService.CreateItemList("trainer", filter, dateSelected);
                _checker.LocationCheck();
                _checker.EquipmentCheck();
                return View(WeekdayList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                var Error = new List<WeekdayItem>();
                Error.Add(new WeekdayItem() { Error = ex.Message });
                Error.Add(_gymclassCalendarService.DropDownOptions(filter, "trainer", dateSelected));
                return View(Error);
            }
        }
        [HttpGet]
        public IActionResult LocationSchedule(string filter, DateTime dateSelected)
        {
            try
            {
                var WeekdayList = _gymclassCalendarService.CreateItemList("location", filter, dateSelected);
                _checker.LocationCheck();
                _checker.EquipmentCheck();
                return View(WeekdayList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                var Error = new List<WeekdayItem>();
                Error.Add(new WeekdayItem() { Error = ex.Message });
                Error.Add(_gymclassCalendarService.DropDownOptions(filter, "location", dateSelected));
                return View(Error);
            }
        }

        [HttpGet]
        public IActionResult Details(GymClassVM gymClassVM) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                _checker.EquipmentCheck();
                _checker.LocationCheck();
                var gymclassVM = _GymclassService.GetItem(x => x.Id == gymClassVM.Id);
                gymclassVM.Error = gymClassVM.Error;
                return View(gymclassVM);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                var gymclassVM = _GymclassService.GetItem(x=> x.Id == gymClassVM.Id);
                if (ex.InnerException != null) { gymclassVM.Error = ex.InnerException.Message; }
                else { gymclassVM.Error = ex.Message; }
                return View();
            }
        }
        public IActionResult AddGymclass(string type)
        {
            _checker.EquipmentCheck();
            _checker.LocationCheck();
            var gymclassVM = _DropdownOptions.DropDownOptions();
            gymclassVM.Type = type;
            gymclassVM.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute,0); //setting reasonable default value
            gymclassVM.EndTime = gymclassVM.Date.AddHours(1);
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
                gymclassVM.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymclassVM.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymclassVM.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                gymclassVM.ClientOptions = _DropdownOptions.DropDownOptions().ClientOptions;
                if (ex.InnerException != null) { gymclassVM.Error = ex.InnerException.Message; }
                else { gymclassVM.Error = ex.Message; }
                return View(gymclassVM);
            }
        }
        public IActionResult EditGymclass(GymClassVM gymClassVM)
        {
            try
            {
                _checker.EquipmentCheck();
                _checker.LocationCheck();
                var gymclass = _GymclassService.GetItem(x => x.Id == gymClassVM.Id);
                gymclass.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymclass.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymclass.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                gymclass.ClientOptions = _DropdownOptions.DropDownOptions().ClientOptions;
                return View(gymclass);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                gymClassVM.LocationOptions = _DropdownOptions.DropDownOptions().LocationOptions;
                gymClassVM.EquipmentOptions = _DropdownOptions.DropDownOptions().EquipmentOptions;
                gymClassVM.EmployeeOptions = _DropdownOptions.DropDownOptions().EmployeeOptions;
                gymClassVM.ClientOptions = _DropdownOptions.DropDownOptions().ClientOptions;
                if (ex.InnerException != null) { gymClassVM.Error = ex.InnerException.Message; }
                else { gymClassVM.Error = ex.Message; }
                return View(gymClassVM);
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
                if (ex.InnerException != null) { gymclassVM.Error = ex.InnerException.Message; }
                else { gymclassVM.Error = ex.Message; }
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
                _GymclassService.DeleteService(gymclassVM);
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
                if (ex.InnerException != null) { gymclassVM.Error = ex.InnerException.Message; }
                else { gymclassVM.Error = ex.Message; }
                return RedirectToAction("Details", "GymClass", new { id = gymclassVM.Id, error = gymclassVM.Error });
            }
        }
    }
}

