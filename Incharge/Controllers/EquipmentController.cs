using Incharge.Service.IService;
using Incharge.Service.PagingService;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutoMapper;
using log4net;
using Incharge.Service;

namespace Incharge.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IService<EquipmentVM, Equipment> _EquipmentService;
        private readonly IPagingService<PaginatedList<Equipment>> _pagingService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public EquipmentController(IService<EquipmentVM, Equipment> EquipmentService, IPagingService<PaginatedList<Equipment>> pagingService, ILog logger, IMapper mapper)
        {
            _EquipmentService = EquipmentService;
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

            return View(_pagingService.IndexPaging(sortOrder, currentFilter, searchString, pageNumber));
        }
        [HttpGet]
        public IActionResult Details(int id) //id will be sent when client profile is clicked. Also when all is working change to async
        {
            try
            {
                return View(_EquipmentService.GetItem(x=>x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult AddEquipment()
        {
            //Check if employee/trainger information is needed on display for when new client account is created
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEquipment(EquipmentVM equipmentVM)
        {
            try
            {
                _EquipmentService.AddService(equipmentVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View();
            }
        }
        public IActionResult EditEquipment(int id)
        {
            try
            {
                return View(_EquipmentService.GetItem(x=>x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditClient(EquipmentVM equipmentVM)
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
