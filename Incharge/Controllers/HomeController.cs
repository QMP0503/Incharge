
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
        private readonly IService<EmployeeVM, Employee> _EmployeeService;
        private readonly IService<ClientVM, Client> _ClientService;


        public HomeController(
                     IService<GymClassVM, Gymclass> gymclassService,
                     IService<EmployeeVM, Employee> employeeService,
                     IService<ClientVM, Client> clientService)
        {
            _GymclassService = gymclassService;
            _EmployeeService = employeeService;
            _ClientService = clientService;
            
        }
        [Authorize]
        [Route("/Home")]
        public IActionResult Index() //make this a dashboard
        {
            var GymClass = _GymclassService.ListItem(x => x.Date.Date == DateTime.Now.Date);
            var Employee = _EmployeeService.ListItem(x => x.Role.Type == "Trainer" && !x.Gymclasses.Any(g => g.Date <= DateTime.Now.Date && g.EndTime >= DateTime.Now.Date));
            var Client = _ClientService.ListItem(x => x.Id > 0);

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
