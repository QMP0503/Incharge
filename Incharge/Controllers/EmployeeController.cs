﻿using Incharge.Service.IService;
using Incharge.Service.PagingService;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace Incharge.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        readonly IService<EmployeeVM, Employee> _EmployeeService;
        readonly IDropDownOptions<EmployeeVM> _EmployeeDropDown;
        readonly IPagingService<PaginatedList<Employee>> _pagingService;
        readonly ILog _logger;
        public EmployeeController(IDropDownOptions<EmployeeVM> EmployeeDropDown, IService<EmployeeVM, Employee> EmployeeService, IPagingService<PaginatedList<Employee>> pagingService, ILog logger)
        {
            _EmployeeService = EmployeeService;
            _pagingService = pagingService;
            _logger = logger;
            _EmployeeDropDown = EmployeeDropDown;
        }
        [HttpGet]
        [Route("/Employee")]

        public IActionResult Index(
                                                 string sortOrder,
                                                 string currentFilter,
                                                 string searchString,
                                                 int? pageNumber,
                                                 int pageSize)
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

            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber, pageSize));
        }

        [HttpGet] //ONLY SALARY IS VISIBLE TO ADMIN ... Let admin view seperate page?
        public IActionResult Details(EmployeeVM employeeVM)
        {
            try
            {
                return View(_EmployeeService.GetItem(x => x.Uuid == employeeVM.Uuid));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        
        public IActionResult AddEmployee()
        {
            return View(_EmployeeDropDown.DropDownOptions());
        }
        [HttpPost] //ONLY VISIBLE TO ADMIN TO HIRE PEOPLE
        public IActionResult AddEmployee( EmployeeVM employeeVM)
        {
           
            try
            {
                _EmployeeService.AddService(employeeVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { employeeVM.Error = ex.InnerException.Message; }
                else { employeeVM.Error = ex.Message; }
                employeeVM.EmployeeTypeOptions = _EmployeeDropDown.DropDownOptions().EmployeeTypeOptions;
                return View(employeeVM);
            }
        }
        public IActionResult UpdateEmloyee(EmployeeVM employeeVM)
        {
            try
            {
                var employee = _EmployeeService.GetItem(x => x.Uuid == employeeVM.Uuid);
                employee.Error = employeeVM.Error;
                return View(employee);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { employeeVM.Error = ex.InnerException.Message; }
                else { employeeVM.Error = ex.Message; }
                employeeVM.EmployeeTypeOptions = _EmployeeDropDown.DropDownOptions().EmployeeTypeOptions;
                return View(employeeVM);
            }
        }

        [HttpPost, ActionName("UpdateEmployee")]
        public IActionResult UpdateEmployeeConfirm(EmployeeVM employeeVM)
        {

            try
            {
                _EmployeeService.UpdateService(employeeVM);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { employeeVM.Error = ex.InnerException.Message; }
                else { employeeVM.Error = ex.Message; }
                employeeVM.EmployeeTypeOptions = _EmployeeDropDown.DropDownOptions().EmployeeTypeOptions;
                return View(employeeVM);
            }
        }


        [HttpPost, ActionName("DeleteEmployee")]
        public IActionResult DeleteEmployeeConfirm(EmployeeVM employeeVM)
        {
            try
            {
                _EmployeeService.DeleteService(employeeVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { employeeVM.Error = ex.InnerException.Message; }
                else { employeeVM.Error = ex.Message; }
                return View(employeeVM);
            }
        }
    }
}
