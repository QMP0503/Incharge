using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Authorization;


namespace Incharge.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly IService<EquipmentVM, Equipment> _EquipmentService;
        private readonly IPagingService<PaginatedList<Equipment>> _pagingService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;
        private readonly IChecker _checker;

        public EquipmentController(IChecker checker, IService<EquipmentVM, Equipment> EquipmentService, IPagingService<PaginatedList<Equipment>> pagingService, ILog logger, IMapper mapper)
        {
            _EquipmentService = EquipmentService;
            _pagingService = pagingService;
            _logger = logger;
            _mapper = mapper;
            _checker = checker;
        }


        [HttpGet]
        [Route("/Equipment")]
        public IActionResult Index(
                                                         string sortOrder,
                                                         string currentFilter,
                                                         string searchString,
                                                         int? pageNumber,
                                                         int pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : string.Empty;
            ViewData["StatusSortParam"] = sortOrder == "Status_asc" ? "Status_desc" : "Status_asc";
            ViewData["GymClassSortParam"] = sortOrder == "GymClass.Name_asc" ? "GymClass.Name_desc" : "GymClass.Name_asc";
            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            _checker.EquipmentCheck();

            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber, pageSize));
        }
        [HttpGet]
        public IActionResult Details(int id) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                var equipment = _EquipmentService.GetItem(x => x.Id == id);
                return View(equipment);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddEquipment()
        {
            return View(new EquipmentVM() { PurchaseDate = DateTime.Now, MaintanceDate = DateTime.Now.AddMonths(1)});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEquipment(EquipmentVM equipmentVM)
        {
            if(!ModelState.IsValid)
            {
                equipmentVM.Error = "Invalid inputs!";
                return View(equipmentVM);
            }
            try
            {
                _EquipmentService.AddService(equipmentVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex.InnerException != null) { equipmentVM.Error = ex.InnerException.Message; }
                else { equipmentVM.Error = ex.Message; }
                return View(equipmentVM);
            }
        }
        public IActionResult EditEquipment(EquipmentVM equipmentVM)
        {
            try
            {
                var equipment = _EquipmentService.GetItem(x => x.Id == equipmentVM.Id);
                equipment.Error = equipmentVM.Error;
                return View(equipment);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                equipmentVM = _EquipmentService.GetItem(x => x.Id == equipmentVM.Id);
                equipmentVM.Error = ex.Message;
                return View(equipmentVM);
            }

        }

        [HttpPost, ActionName("EditEquipment")]
        [ValidateAntiForgeryToken]
        public IActionResult EditEquipmentConfirm(EquipmentVM equipmentVM)
        {
            try
            {
                _EquipmentService.UpdateService(equipmentVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                equipmentVM.Error = ex.InnerException.Message ?? ex.Message;
                return View(equipmentVM);
            }
        }

        public IActionResult DeleteEquipment(int id)
        {
            try
            {
                return View(_EquipmentService.GetItem(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }

        [HttpPost, ActionName("DeleteEquipment")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEquipmentConfirm(EquipmentVM equipmentVM)
        {
            try
            {
                _EquipmentService.UpdateService(equipmentVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }
        }
        [HttpPost, ActionName("UpdateStatus")]
        public IActionResult UpdateStatus(EquipmentVM equipmentVM)//dont need bind if html id is used explicitly
        {
            try
            {
                _EquipmentService.UpdateService(equipmentVM);
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
