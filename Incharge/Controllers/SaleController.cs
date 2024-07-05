using CloudinaryDotNet.Actions;
using Incharge.Models;
using Incharge.Service.IService;
using Incharge.ViewModels;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace Incharge.Controllers
{
    public class SaleController : Controller
    {
        private readonly IService<SaleVM, Sale> _SaleService;
        private readonly IDropDownOptions<SaleVM> _SaleDropDown; 
        private readonly ILog _logger;

        public SaleController(IService<SaleVM, Sale> SaleService, ILog logger, IDropDownOptions<SaleVM> dropDownOptions)
        {
            _SaleService = SaleService;
            _logger = logger;
            _SaleDropDown = dropDownOptions;
        }
        public IActionResult Index()
        {
            return View();
        }

        //CURRENTLY BROKEN
        //public IActionResult _SalePartial() 
        //{
        //    return PartialView("_SalePartial", _SaleDropDown.DropDownOptions()); //just to print out the values
        //}

        public IActionResult AddSale(SaleVM saleVM)
        {
            saleVM.ClientOptions = _SaleDropDown.DropDownOptions().ClientOptions;
            saleVM.EmployeeOptions = _SaleDropDown.DropDownOptions().EmployeeOptions;
            saleVM.ProductOptions = _SaleDropDown.DropDownOptions().ProductOptions;
            saleVM.Date=DateTime.Now;
            return View(saleVM); //for ease of selection
        }

        [HttpPost, ActionName("AddSale")]
        public IActionResult AddSaleConfirm(SaleVM saleVM)
        {
            try
            {
                _SaleService.AddService(saleVM);
                return RedirectToAction("Index","Product"); //lead back to product cause that is the only place u can add a sale
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View(NotFound()); //Find logical location if fail instead of error 404
            }
        }
    }
}
