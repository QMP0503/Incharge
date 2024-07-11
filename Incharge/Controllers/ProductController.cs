using Microsoft.AspNetCore.Mvc;
using Incharge.Service.IService;
using Incharge.ViewModels;
using Incharge.Models;
using log4net;
using Incharge.Service.PagingService;

namespace Incharge.Controllers
{
    public class ProductController : Controller
    {
        readonly IService<ProductVM, Product> _ProductService;
        readonly IPagingService<PaginatedList<Product>> _pagingService;
        readonly IDropDownOptions<ProductVM> _productDropDown;
        readonly ILog _logger;

        public ProductController(IDropDownOptions<ProductVM> ProductDropDown ,IService<ProductVM, Product> ProductService, IPagingService<PaginatedList<Product>> pagingService, ILog logger)
        {
            _ProductService = ProductService;
            _pagingService = pagingService;
            _productDropDown = ProductDropDown;
            _logger = logger;
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
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : string.Empty;
            ViewData["PriceSortParam"] = sortOrder == "Price" ? "Price_desc" : "Price";

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
        
        [HttpGet]
        public IActionResult Details(int id) 
        {
            try
            {
                var product = _ProductService.GetItem(x => x.Id == id);
                product.ProductTypeOption = _productDropDown.DropDownOptions().ProductTypeOption;
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddProduct()
        {
            return View(_productDropDown.DropDownOptions()); //just for the product type drop down menu
        }

        [HttpPost, ActionName("AddProduct")]
        public IActionResult AddProduct(ProductVM productVM)
        {
            try
            {
                _ProductService.AddService(productVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult UpdateProduct(int id)
        {
            try
            {
                var productVM = _ProductService.GetItem(x => x.Id == id);
                productVM.ProductTypeOption = _productDropDown.DropDownOptions().ProductTypeOption;
                return View(productVM);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        [HttpPost]
        public IActionResult UpdateProduct(ProductVM productVM)
        {
            try
            {
                _ProductService.UpdateService(productVM);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        ////make into partial view
        //public IActionResult DeleteProduct(int id)
        //{
        //    try
        //    {
        //        return PartialView("_DeleteConfirmationPartial", _ProductService.GetItem(x => x.Id == id)); //product serive is the model that is 
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex);
        //        return View();
        //    }
        //}

        //trying to make partial view for delete confirmation page
        [HttpPost] //delete with confirmation paper pop-up in page
        public IActionResult DeleteProduct(ProductVM productVM)
        {
            try
            {
                _ProductService.DeleteService(productVM); //should only have the ID value.
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }


        public IActionResult RegisterClient(int id)
        {
            try
            {
                return View(_productDropDown.DropDownOptions());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }

        [HttpPost, ActionName("RegisterClient")] //should only update 
        public IActionResult RegisterClient(ProductVM productVM) //need to make sales controller and method to register the transaction
        {
            try
            {
                _ProductService.UpdateService(productVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View(); //link to error page when possible
            }
        }
		//decide where to put product assigned to client.
		[HttpPost]
		public IActionResult AddProductToClient(ProductVM productVM) //only have list of clients
		{
			try
			{
				_ProductService.UpdateService(productVM);
				return RedirectToAction(nameof(Index)); //find a more logical place to redirect
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return View();
			}
		}
	}
}
