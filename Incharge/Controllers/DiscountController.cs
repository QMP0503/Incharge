using Microsoft.AspNetCore.Mvc;
using Incharge.Models;
using Incharge.ViewModels;
using Incharge.Service.IService;
using Incharge.Service.PagingService;
using log4net;
using Microsoft.AspNetCore.Authorization;

namespace Incharge.Controllers
{
    public class DiscountController : Controller
    {
        readonly ILog _logger;
        readonly IService<DiscountVM, Discount> _DiscountService;
        private readonly IPagingService<PaginatedList<Discount>> _pagingService;

        public DiscountController(ILog logger, IService<DiscountVM, Discount> DiscountService, IPagingService<PaginatedList<Discount>> pagingService)
        {
            _logger = logger;
            _DiscountService = DiscountService;
            _pagingService = pagingService;
        }

        [HttpGet] //will be the index
        public IActionResult Index(
                                                         string sortOrder,
                                                         string currentFilter,
                                                         string searchString,
                                                         int? pageNumber,
                                                         int pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DiscountValueParam"] = string.IsNullOrEmpty(sortOrder) ? "DiscountValue_desc" : string.Empty;
            ViewData["NameSortParam"] = sortOrder == "Name_desc" ? "Name_asc" : "Name_desc";

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

        public IActionResult AddDiscount(DiscountVM discountVM)
        {
            return View(discountVM); //for ease of selection
        }

        [HttpPost, ActionName("AddDiscount")]
        public IActionResult AddDiscountConfirm(DiscountVM discountVM)
        {
            try
            {
                _DiscountService.AddService(discountVM);
                return RedirectToAction("Index", "Discount"); //lead back to product cause that is the only place u can add a sale
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { discountVM.Error = ex.InnerException.Message; }
                else { discountVM.Error = ex.Message; }
                return View(discountVM);
            }
        }
        [HttpPost, ActionName("DeleteDiscount")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteDiscountConfirm(DiscountVM discountVM)
        {
            try
            {
                _DiscountService.DeleteService(discountVM);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View(NotFound());
            }
        }

        //should only be used by ADMIN
        public IActionResult EditDiscount(DiscountVM discountVM)
        {
            var DiscountVM = _DiscountService.GetItem(x => x.Id == discountVM.Id);
            return View(DiscountVM); //for ease of selection
        }

        [HttpPost, ActionName("EditDiscount")]
        public IActionResult EditDiscountConfirm(DiscountVM discountVM)
        {
            try
            {
                _DiscountService.UpdateService(discountVM);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { discountVM.Error = ex.InnerException.Message; }
                else { discountVM.Error = ex.Message; }
                return View(discountVM);
            }
        }
    }
}
