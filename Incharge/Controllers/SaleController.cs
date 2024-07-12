using CloudinaryDotNet.Actions;
using Incharge.Models;
using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly IService<SaleVM, Sale> _SaleService;
        private readonly IBusinessReportService _BusinessReportService;
        private readonly IDropDownOptions<SaleVM> _SaleDropDown;
        private readonly IPagingService<PaginatedList<Sale>> _PagingService;
        private readonly ILog _logger;

        public SaleController(IPagingService<PaginatedList<Sale>> pagingService, IService<SaleVM, Sale> SaleService, ILog logger, IDropDownOptions<SaleVM> dropDownOptions, IBusinessReportService businessReportService)
        {
            _SaleService = SaleService;
            _logger = logger;
            _SaleDropDown = dropDownOptions;
            _BusinessReportService = businessReportService;
            _PagingService = pagingService;
        }

        [HttpGet]
        public IActionResult Index(
                                                string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? pageNumber,
                                                int pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Date_asc" : string.Empty;
            ViewData["ProductNameSortParam"] = sortOrder == "ProductName_asc" ? "ProductName_desc" : "ProductName_asc";
            ViewData["PaymentTypeSortParam"] = sortOrder == "PaymentType_asc" ? "PaymentType_desc" : "PaymentType_asc";
            ViewData["TotalPriceSortParam"] = sortOrder == "TotalPrice_asc" ? "TotalPrice_desc" : "TotalPrice_asc";


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

        public IActionResult AddSale(SaleVM saleVM)
        {
            saleVM.ClientOptions = _SaleDropDown.DropDownOptions().ClientOptions;
            saleVM.EmployeeOptions = _SaleDropDown.DropDownOptions().EmployeeOptions;
            saleVM.ProductOptions = _SaleDropDown.DropDownOptions().ProductOptions;
            saleVM.DiscountOptions = _SaleDropDown.DropDownOptions().DiscountOptions;
            saleVM.Date=DateTime.Now;
            return View(saleVM); //for ease of selection
        }

        [HttpPost, ActionName("AddSale")]
        public IActionResult AddSaleConfirm(SaleVM saleVM)
        {
            try
            {
                _SaleService.AddService(saleVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction("Index", "Product"); //lead back to product cause that is the only place u can add a sale
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View(NotFound()); //Find logical location if fail instead of error 404
            }
        }
        [HttpPost, ActionName("DeleteSale")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSaleConfirm(SaleVM saleVM)
        {
            try
            {
                _SaleService.DeleteService(saleVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View(NotFound());
            }
        }

        //should only be used by ADMIN
        public IActionResult EditSale(SaleVM saleVM)
        {
            var sale = _SaleService.GetItem(x => x.Uuid == saleVM.Uuid);
            sale.ClientOptions = _SaleDropDown.DropDownOptions().ClientOptions;
            sale.EmployeeOptions = _SaleDropDown.DropDownOptions().EmployeeOptions;
            sale.ProductOptions = _SaleDropDown.DropDownOptions().ProductOptions;
            sale.DiscountOptions = _SaleDropDown.DropDownOptions().DiscountOptions;
            sale.Date = DateTime.Now;
            return View(sale); //for ease of selection
        }

        [HttpPost, ActionName("EditSale")]
        public IActionResult EditSaleConfirm(SaleVM saleVM)
        {
            try
            {
                _SaleService.UpdateService(saleVM);
                _BusinessReportService.UpdateService();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View(NotFound());
            }
        }
    }
}
