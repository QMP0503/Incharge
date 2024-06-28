using Incharge.Service.IService;
using Incharge.Service.PagingService;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Controllers
{
    public class EmployeeController : Controller
    {
        readonly IEmployeeService _EmployeeService;
        readonly IPagingService<PaginatedList<Employee>> _pagingService;
        readonly ILog _logger;
        public EmployeeController(IEmployeeService EmployeeService, IPagingService<PaginatedList<Employee>> pagingService, ILog logger)
        {
            _EmployeeService = EmployeeService;
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
        [HttpGet] //ONLY SALARY IS VISIBLE TO ADMIN ... Let admin view seperate page?
        public IActionResult Details(string Uuid)
        {
            try
            {
                var employeeInfo = new EmployeeVM();
                employeeInfo.Uuid = Uuid;
                return View(_EmployeeService.FindEmployee(employeeInfo));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        [HttpPost] //ONLY VISIBLE TO ADMIN TO HIRE PEOPLE
        public IActionResult AddEmployee([Bind("FirstName, LastName, TotalSalary, Email, RoleId")] EmployeeVM employeeVM)
        {
            try
            {
                _EmployeeService.AddEmployee(employeeVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult UpdateEmloyee(string Uuid)
        {
            try
            {
                var employeeInfo = new EmployeeVM();
                employeeInfo.Uuid = Uuid;
                return View(_EmployeeService.FindEmployee(employeeInfo));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        [HttpPost]
        public IActionResult UpdateEmployee([Bind("Uuid, FirstName, LastName, TotalSalary, Email, RoleId")] EmployeeVM employeeVM)
        {
            try
            {
                _EmployeeService.UpdateEmployee(employeeVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        //test if view page is needed
        [HttpPost]
        public IActionResult AddClientToEmployee([Bind("Uuid, ClientId")] EmployeeVM employeeVM)
        {
            try
            {
                _EmployeeService.AddClientToEmployee(employeeVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        //check if we need view page

        [HttpPost]
        public IActionResult AddGymClassToEmployee([Bind("Uuid, GymClassId")] EmployeeVM employeeVM)
        {
            try
            {
                _EmployeeService.AddGymClassToEmployee(employeeVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        //view page for delete employee
        public IActionResult DeleteEmployee(string Uuid)
        {
            try
            {
                var employeeInfo = new EmployeeVM();
                employeeInfo.Uuid = Uuid;
                return View(_EmployeeService.FindEmployee(employeeInfo));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        [HttpPost, ActionName("DeleteEmployee")]
        public IActionResult DeleteEmployeeConfirm(string Uuid)
        {
            try
            {
                _EmployeeService.DeleteEmployee(Uuid);
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
