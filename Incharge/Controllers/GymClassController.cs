using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using log4net;

namespace Incharge.Controllers
{
    public class GymclassController : Controller
    {
        private readonly IService<GymClassVM, Gymclass> _GymclassService;
        private readonly IPagingService<PaginatedList<Gymclass>> _pagingService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public GymclassController(IService<GymClassVM, Gymclass> GymclassService, IPagingService<PaginatedList<Gymclass>> pagingService, ILog logger, IMapper mapper)
        {
            _GymclassService = GymclassService;
            _pagingService = pagingService;
            _logger = logger;
            _mapper = mapper;
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
            ViewData["ClassDateSortParam"] = sortOrder == "ClassDate_desc" ? "ClassDate_asc" : "ClassDate_desc";

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
        public IActionResult Details(int id) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                return View(_GymclassService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddGymclass()
        {
            //Check if employee/trainger information is needed on display for when new client account is created
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGymclass(GymClassVM gymclassVM)
        {
            try
            {
                _GymclassService.AddService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult EditGymclass(int id)
        {
            try
            {
                return View(_GymclassService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditClient(GymClassVM gymclassVM)
        {
            try
            {
                _GymclassService.UpdateService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        public IActionResult DeleteGymClass(int id)
        {
            try
            {
                return View(_GymclassService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }

        [HttpPost, ActionName("DeleteGymClass")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGymClassConfirm(GymClassVM gymclassVM)
        {
            try
            {
                _GymclassService.UpdateService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        [HttpPost, ActionName("UpdateStatus")]
        public IActionResult UpdateStatus(GymClassVM gymclassVM)//dont need bind if html id is used explicitly
        {
            try
            {
                _GymclassService.UpdateService(gymclassVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
    }
}
