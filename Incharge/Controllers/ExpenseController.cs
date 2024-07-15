using Microsoft.AspNetCore.Mvc;
using Incharge.Service.IService;
using Incharge.Models;
using Incharge.ViewModels;
using log4net;
using Incharge.Service.PagingService;
using Microsoft.AspNetCore.Authorization;
using MySqlX.XDevAPI;


namespace Incharge.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ILog _log;
        private readonly IService<ExpenseVM, Expense> _ExpenseService;
        private readonly IBusinessReportService _BusinessReportService;
        private readonly IPagingService<PaginatedList<Expense>> _PagingService;

        public ExpenseController(IService<ExpenseVM, Expense> ExpenseService, ILog logger, IBusinessReportService businessReportService, IPagingService<PaginatedList<Expense>> PagingService)
        {
            _ExpenseService = ExpenseService;
            _log = logger;
            _BusinessReportService = businessReportService;
            _PagingService = PagingService;
        }

        [HttpGet] //make /index be just name of controller
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


        public IActionResult AddExpense()
        {
            var expenseVM = new ExpenseVM();
            return View(expenseVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExpense(ExpenseVM expenseVM)
        {
            if (!ModelState.IsValid)
            {
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
                return View();
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
                return View();
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
                return View();
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
    }
}
