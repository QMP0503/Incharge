using Microsoft.AspNetCore.Mvc;
using Incharge.Service.IService;
using Incharge.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;

namespace Incharge.Controllers
{
    [Authorize(Roles = "Admin")] //only for admin/business owners to see
    public class AnalyticsController : Controller
    {
        private readonly IBusinessReportService _BusinessReportService;
        private readonly ILog _logger;

        public AnalyticsController(IBusinessReportService businessReportService, ILog logger)
        {
            _BusinessReportService = businessReportService;
            _logger = logger;
        }

        [HttpGet] //torn between making them get a whole list of past years and summarize it or just display
        public IActionResult Index()
        {
            return View();
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

    }
}
