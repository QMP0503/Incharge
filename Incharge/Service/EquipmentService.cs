using AutoMapper;
using Incharge.Data;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.ViewModels;

namespace Incharge.Service
{
    public class EquipmentService : IService<EquipmentVM, Equipment>//use autopmapper
    {
        private readonly IFindRepository<Equipment> _FindEquipmentRepository;
        private readonly IRepository<Equipment> _EquipmentRepository;
        private readonly IMapper _Mapper;
        public EquipmentService(IMapper mapper, IFindRepository<Equipment> FindEquipmentRepository, IRepository<Equipment> EquipmentRepository)
        {
            _FindEquipmentRepository = FindEquipmentRepository;
            _EquipmentRepository = EquipmentRepository;
            _Mapper = mapper;
        }
        public List<EquipmentVM> ListItem(Func<Equipment, bool> predicate)//mapper
        {
            var equipmentList = _FindEquipmentRepository.ListBy(predicate);
            var equipmentVMList = _Mapper.Map<List<EquipmentVM>>(equipmentList);
            return equipmentVMList;
        }
        public EquipmentVM GetItem(Func<Equipment, bool> predicate)
        {
            var equipment = _FindEquipmentRepository.FindBy(predicate);
            var equipmentVM = _Mapper.Map<EquipmentVM>(equipment);
            return equipmentVM;
        }

        public void AddService(EquipmentVM equipmentVM) //make sure no null data point error
        {
            if (equipmentVM == null) { throw new NullReferenceException("Input Empty."); }
            var equipment = new Equipment
            {
                Name = equipmentVM.Name,
                Description = equipmentVM.Description,
                PurchaseDate = equipmentVM.PurchaseDate,
                MaintanceDate = equipmentVM.MaintanceDate,
                Status = "available"
            };
            _EquipmentRepository.Add(equipment);
            _EquipmentRepository.Save();

        }
        public void UpdateService(EquipmentVM equipmentVM)
        {
            if (equipmentVM == null)
            {
                throw new NullReferenceException("Input Empty.");
            }

            var equipment = _FindEquipmentRepository.FindBy(x => x.Id == equipmentVM.Id);

            if (equipment == null)
            {
                throw new NullReferenceException("Equipment Empty.");
            }

            equipment.Name = equipmentVM.Name ?? equipment.Name;
            equipment.Description = equipmentVM.Description ?? equipment.Description;
            equipment.PurchaseDate = equipmentVM.PurchaseDate ?? equipment.PurchaseDate;
            equipment.MaintanceDate = equipmentVM.MaintanceDate ?? equipment.MaintanceDate;
            equipment.Status = equipmentVM.Status ?? equipment.Status;
            equipment.GymClassId = equipmentVM.GymClassId ?? equipment.GymClassId;
            equipment.GymClass = equipmentVM.GymClass ?? equipment.GymClass;
            _EquipmentRepository.Update(equipment);
            _EquipmentRepository.Save();
        }

        public void DeleteService(EquipmentVM equipmentVM)
        {
            if(equipmentVM.Id < 1) { throw new NullReferenceException("Input Invalid."); }
            var equipmentToDelete = _FindEquipmentRepository.FindBy(x => x.Id == equipmentVM.Id);
            if (equipmentToDelete == null) { throw new NullReferenceException("Equipment Empty."); }
            _EquipmentRepository.Delete(equipmentToDelete);
            _EquipmentRepository.Save();
        }
    }
}
