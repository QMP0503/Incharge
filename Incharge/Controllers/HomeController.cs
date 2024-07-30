
using AutoMapper;
using Incharge.Models;
using Incharge.Service.IService;
using Incharge.ViewModels;
using log4net;
using Microsoft.AspNetCore.Mvc;


namespace Incharge.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<GymClassVM, Gymclass> _GymclassService;
        private readonly IService<LocationVM, Location> _LocationService;
        private readonly IService<EmployeeVM, Employee> _EmployeeService;
        private readonly IService<ClientVM, Client> _ClientService;
        private readonly IGymclassCalendarService _GymclassCalendarService;
        private readonly IService<EquipmentVM, Equipment> _EquipmentService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public HomeController(ILog logger,
                     IService<GymClassVM, Gymclass> gymclassService,
                     IService<LocationVM, Location> locationService,
                     IService<EmployeeVM, Employee> employeeService,
                     IService<ClientVM, Client> clientService,
                     IGymclassCalendarService gymclassCalendarService,
                     IService<EquipmentVM, Equipment> equipmentService,
                     IMapper mapper)
        {
            _logger = logger;
            _GymclassService = gymclassService;
            _LocationService = locationService;
            _EmployeeService = employeeService;
            _ClientService = clientService;
            _GymclassCalendarService = gymclassCalendarService;
            _EquipmentService = equipmentService;
            _mapper = mapper;
        }

        public IActionResult Index() //make this a dashboard
        {
            var homeVM = new HomeVM()
            {
                GymClasses = _GymclassService.ListItem(x => x.Date.Date == DateTime.Now.Date),
                Locations = _LocationService.ListItem(x => x.Id > 0),
                Employees = _EmployeeService.ListItem(x => x.Role.Type == "Trainer"),
                Clients = _ClientService.ListItem(x => x.Id > 0),
                Equipment = _EquipmentService.ListItem(x => x.Id > 0)
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
