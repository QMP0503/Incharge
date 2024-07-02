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
        readonly ILog _logger;

        public ProductController(IService<ProductVM, Product> ProductService, IPagingService<PaginatedList<Product>> pagingService, ILog logger)
        {
            _ProductService = ProductService;
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

            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber));
        }
        
        [HttpGet]
        public IActionResult Details(int id) 
        {
            try
            {
                return View(_ProductService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        [HttpPost]
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
                return View(_ProductService.GetItem(x => x.Id == id));
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
        //decide where to put product assigned to client.
        [HttpPost]
        public IActionResult AddProductToClient(ProductVM productVM) //only have list of clients
        {
            try
            {
                _ProductService.UpdateService(productVM);
                return RedirectToAction(nameof(Index)); //find a more logical place to redirect
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                return View(_ProductService.GetItem(x => x.Id == id));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        [HttpPost, ActionName("DeleteProduct")]
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
    }
}
