using Microsoft.AspNetCore.Mvc;
using Incharge.Service.IService;
using Incharge.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Incharge.Models;
using Incharge.Service.PagingService;

namespace Incharge.Controllers
{
    [Authorize(Roles = "Admin")] //only for admin/business owners to see
    public class AnalyticsController : Controller
    {
        private readonly IBusinessReportService _BusinessReportService;
        private readonly ILog _logger;
        private readonly IPagingService<PaginatedList<BusinessReport>> _pagingService;

        public AnalyticsController(IBusinessReportService businessReportService, ILog logger, IPagingService<PaginatedList<BusinessReport>> pagingService)
        {
            _BusinessReportService = businessReportService;
            _logger = logger;
            _pagingService = pagingService;
        }

        [HttpGet]
        [Route("/Analytics")]
        public IActionResult Index()
        {
            try
            {
                var YearbusinessReportVM = _BusinessReportService.ListItem(x => x.Date.Year == DateTime.Now.Year);
                _BusinessReportService.UpdateService();
                return View(YearbusinessReportVM);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                var YearbusinessReportVM = _BusinessReportService.ListItem(x => x.Date.Year == DateTime.Now.Year);
                if (ex.InnerException != null) { YearbusinessReportVM.First().Error = ex.InnerException.Message; }
                else { YearbusinessReportVM.First().Error = ex.Message; }
                return View(YearbusinessReportVM);
            }

        }


        [HttpGet] //display summarized information for the month report
        public IActionResult Detail(BusinessReportVM businessReportVM)
        {
            try
            {
                return View(_BusinessReportService.GetItem(x => x.Uuid == businessReportVM.Uuid));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        [HttpPost]
        public IActionResult DeleteReport(BusinessReportVM businessReportVM)
        {
            try
            {
                _BusinessReportService.DeleteService(businessReportVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }

        }

        [HttpGet]
        public IActionResult ReportList(
                                                 string sortOrder,
                                                 string currentFilter,
                                                 string searchString,
                                                 int? pageNumber,
                                                 int pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Date_asc" : string.Empty; ;
            ViewData["RevenueSortParam"] = sortOrder == "Revenue_asc" ? "Revenue_desc" : "Revenue_asc";
            ViewData["CostSortParam"] = sortOrder == "Cost_asc" ? "Cost_desc" : "Cost_asc";
            ViewData["ActiveMembersSortParam"] = sortOrder == "ActiveMembers_asc" ? "ActiveMembers_desc" : "ActiveMembers_asc";
            ViewData["NewMembershipsSoldSortParam"] = sortOrder == "NewMembershipsSold_asc" ? "NewMembershipsSold_desc" : "NewMembershipsSold_asc";
            ViewData["ProfitsSortParam"] = sortOrder == "Profits_asc" ? "Profits_desc" : "Profits_asc";


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
    }
}
