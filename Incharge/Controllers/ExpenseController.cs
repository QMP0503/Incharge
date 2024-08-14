using Microsoft.AspNetCore.Mvc;
using Incharge.Service.IService;
using Incharge.Models;
using Incharge.ViewModels;
using log4net;
using Incharge.Service.PagingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;



namespace Incharge.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ILog _log;
        private readonly IService<ExpenseVM, Expense> _ExpenseService;
        private readonly IService<EmployeeVM, Employee> _EmployeeRepository;
        private readonly IBusinessReportService _BusinessReportService;
        private readonly IPagingService<PaginatedList<Expense>> _PagingService;
        private readonly IRecurringExpenseService _RecurringExpenseService;

        public ExpenseController(IService<EmployeeVM, Employee> EmployeeRepository, IRecurringExpenseService RecurringExpenseService, IService<ExpenseVM, Expense> ExpenseService, ILog logger, IBusinessReportService businessReportService, IPagingService<PaginatedList<Expense>> PagingService)
        {
            _ExpenseService = ExpenseService;
            _log = logger;
            _BusinessReportService = businessReportService;
            _PagingService = PagingService;
            _RecurringExpenseService = RecurringExpenseService;
            _EmployeeRepository = EmployeeRepository;
        }

        [HttpGet] //make index be just name of controller
        [Route("/Expense")]
        public IActionResult Index(
                                                 string sortOrder,
                                                 string currentFilter,
                                                 string searchString,
                                                 int? pageNumber, 
                                                 int pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Date_asc" : string.Empty;
            ViewData["CostSortParam"] = sortOrder == "Cost_asc" ? "Cost_desc" : "Cost_asc";
            ViewData["NameSortParam"] = sortOrder == "Name_asc" ? "Name_desc" : "Name_asc";
            ViewData["TypeSortParam"] = sortOrder == "Type_asc" ? "Type_desc" : "Type_asc";

            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            return View(_PagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber, pageSize));
        }


        //For one time payments only
        public IActionResult AddExpense(ExpenseVM expenseVM)
        {
            return View(expenseVM);
        }

        [HttpPost, ActionName("AddExpense")]
        [ValidateAntiForgeryToken]
        public IActionResult AddExpenseConfirm(ExpenseVM expenseVM)
        {
            if (!ModelState.IsValid)
            {
                //Add method to see where the error is
                expenseVM.Error = "Invalid inputs";
                return View(expenseVM);
            }
            try
            {
                _ExpenseService.AddService(expenseVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                if(ex.InnerException != null) { expenseVM.Error = ex.InnerException.Message; }
                else { expenseVM.Error = ex.Message; }
                return View(expenseVM);
            }
        }

        public IActionResult EditExpense(ExpenseVM expenseVM)
        {
            try
            {
                return View(_ExpenseService.GetItem(x=> x.Uuid == expenseVM.Uuid));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View(NotFound()); //very lazy but figure out later
            }
        }

        [HttpPost, ActionName("EditExpense")]
        [ValidateAntiForgeryToken]
        public IActionResult EditExpenseConfirm(ExpenseVM expenseVM)
        {
            if (!ModelState.IsValid)
            {
                expenseVM.Error = "Invalid inputs";
                return RedirectToAction("Details", new { uuid = expenseVM.Uuid, error = expenseVM.Error });
            }
            try
            {
                _ExpenseService.UpdateService(expenseVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                if (ex.InnerException != null) { expenseVM.Error = ex.InnerException.Message; }
                else { expenseVM.Error = ex.Message; }
                return View(expenseVM);
            }
        }

        //no need for view page
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteExpense(ExpenseVM expenseVM)
        {
            try
            {
                _ExpenseService.DeleteService(expenseVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View();
            }
        }

        public IActionResult RecurringExpense(string Option) //the option would determine what options to show
        {
            var expenseVM = new ExpenseVM();
            try
            {
                switch (Option)
                {
                    //for cases below the program would get expense from last month and copy as new expense
                    //have checker to make sure rent payment is only made once a month
                    case "Rent":
                        if (_ExpenseService.GetItem(x => x.Date.Month == DateTime.Now.Month && x.Type == "Rent") != null)
                        {
                             throw new Exception ($"Rent have already been paid");
                        }
                        expenseVM.RecurringList = _ExpenseService.ListItem(x => x.Date.Month == DateTime.Now.AddMonths(-1).Month && x.Type == "Rent");
                        if (expenseVM.RecurringList.Count() == 0)
                        {
                            return RedirectToAction(nameof(AddExpense), new ExpenseVM() { Type = "Rent", Name = "Rent" });
                        }
                        expenseVM.Type = "Rent";
                        return View(expenseVM);

                    case "Utilities":
                        if (_ExpenseService.GetItem(x => x.Date.Month == DateTime.Now.Month && x.Type == "Utilities") != null)
                        {
                            throw new Exception($"Utility have already been paid");
                        }
                        expenseVM.RecurringList = _ExpenseService.ListItem(x => x.Date.Month == DateTime.Now.AddMonths(-1).Month && x.Type == "Utilities");
                        if (expenseVM.RecurringList.Count() == 0)
                        {
                            return RedirectToAction(nameof(AddExpense), new ExpenseVM() { Type = "Utilities" });
                        }
                        expenseVM.Type = "Utilities";
                        return View(expenseVM);

                    //in the event there is multiple type of insurance will show them all as name and price
                    //for sake of simplicity assume only one lump sum for insurance is made
                    case "Insurance":
                        if (_ExpenseService.GetItem(x => x.Date.Month == DateTime.Now.Month && x.Type == "Insurance") != null)
                        {
                            throw new Exception($"Insurance have already been paid");
                        }
                        expenseVM.RecurringList = _ExpenseService.ListItem(x => x.Date.Month == DateTime.Now.AddMonths(-1).Month && x.Type == "Insurance");
                        if (expenseVM.RecurringList.Count() == 0)
                        {
                            return RedirectToAction(nameof(AddExpense), new ExpenseVM() { Type = "Insurance" });
                        }
                        expenseVM.Type = "Insurance";
                        return View(expenseVM);

                    default: //should never be triggered
                        return View(NotFound());
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                expenseVM.Error = ex.Message;
                expenseVM.RecurringList = new List<ExpenseVM>();
                return View(expenseVM);
            }

        }

        [HttpPost]
        [ActionName("RecurringExpense")]
        //have hidden input with the expense type for easier identification
        public IActionResult RecurringExpenseConfirm(ExpenseVM expenseVM)
        {
            try
            {
                _RecurringExpenseService.AddRecurringExpense(expenseVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                if (ex.InnerException != null) { expenseVM.Error = ex.InnerException.Message; }
                else { expenseVM.Error = ex.Message; }
                return View(expenseVM);
            }
        }
        public IActionResult RecurringWages(ExpenseVM expenseVM)
        {
            expenseVM.EmployeeList = new List<EmployeeVM>();
            try
            {
                foreach (var employee in _EmployeeRepository.ListItem(x => x.Id > 0))
                {
                    if(_ExpenseService.GetItem(x => (x.Name.Contains(employee.FirstName) && x.Name.Contains(employee.LastName)) && x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year) == null)
                    {
                        expenseVM.EmployeeList.Add(employee);
                    }
                }
                expenseVM.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if(expenseVM.EmployeeList == null)
                {
                    throw new Exception("All Employees Have been Paid! Please leave the page");
                }
                return View(expenseVM);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                expenseVM.Error = ex.Message;
                expenseVM.EmployeeList = new List<EmployeeVM>();
                //expenseVM.EmployeeList.Add(new EmployeeVM() { FirstName = "All", LastName = "Employees Have Been Paid." });
                return View(expenseVM);
            }
        }
        [HttpPost, ActionName("RecurringWages")]
        public IActionResult RecurringWagesConfirm(ExpenseVM expenseVM) //seperate input for wage in the event of error
        {
            //Get employee list and make new entry for each one
            try
            {
                _RecurringExpenseService.AddWageExpense(expenseVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                if (ex.InnerException != null) { expenseVM.Error = ex.InnerException.Message; }
                else { expenseVM.Error = ex.Message; }
                expenseVM.EmployeeList = _EmployeeRepository.ListItem(x => x.Id > 0);
                expenseVM.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                return View(expenseVM);
            }
        }
    }
}
