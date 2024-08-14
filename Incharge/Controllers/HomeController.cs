
using AutoMapper;
using Incharge.Models;
using Incharge.Service.IService;
using Incharge.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;


namespace Incharge.Controllers
{
    [Authorize]
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
        [Authorize]
        [Route("/Home")]
        public IActionResult Index() //make this a dashboard
        {
            var GymClass = _GymclassService.ListItem(x => x.Date.Date == DateTime.Now.Date);
         // var Location = _LocationService.ListItem(x => x.Id > 0);
            var Employee = _EmployeeService.ListItem(x => x.Role.Type == "Trainer" && !x.Gymclasses.Any(g => g.Date <= DateTime.Now.Date && g.EndTime >= DateTime.Now.Date));
            var Client = _ClientService.ListItem(x => x.Id > 0);
         // var Equipment = _EquipmentService.ListItem(x => x.Id > 0);

            var homeVM = new HomeVM()
            {
                GymClasses = GymClass.Take(5).ToList(),
                Employees = Employee.ToList(),
                Clients = Client,
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
