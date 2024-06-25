using Incharge.Data;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.ViewModels;

namespace Incharge.Service
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IFindRepository<Equipment> _FindEquipmentRepository;
        private readonly IRepository<Equipment> _EquipmentRepository;
        public EquipmentService(IFindRepository<Equipment> FindEquipmentRepository, IRepository<Equipment> EquipmentRepository)
        {
            _FindEquipmentRepository = FindEquipmentRepository;
            _EquipmentRepository = EquipmentRepository;
        }
        public List<Equipment> ListEquipment(Func<Equipment, bool> predicate)
        {
            return _FindEquipmentRepository.ListBy(predicate);
        }
        public Equipment FindEquipment(Func<Equipment, bool> predicate)
        {
            return _FindEquipmentRepository.FindBy(predicate);
        }

        public void AddEquipment(EquipmentVM equipmentVM)
        {
            if (equipmentVM == null) { throw new NullReferenceException("Input Empty."); }
            var equipment = new Equipment
            {
                Name = equipmentVM.Name,
                Description = equipmentVM.Description,
                PurchaseDate = equipmentVM.PurchaseDate,
                MaintanceDate = equipmentVM.MainationDate,
                Status = "available"
            };
            _EquipmentRepository.Add(equipment);
            _EquipmentRepository.Save();

        }
        public void UpdateEquipment(EquipmentVM equipmentVM)
        {
            if(equipmentVM == null) { throw new NullReferenceException("Input Empty."); }
            var equipment = _FindEquipmentRepository.FindBy(x => x.Id == equipmentVM.Id);
            if (equipment == null) { throw new NullReferenceException("Equipment Empty."); }
            equipment.Name = equipmentVM.Name;
            equipment.Description = equipmentVM.Description;
            equipment.PurchaseDate = equipmentVM.PurchaseDate;
            equipment.MaintanceDate = equipmentVM.MainationDate;
            _EquipmentRepository.Update(equipment);
            _EquipmentRepository.Save();
        }
        public void DeleteEquipment(int id)
        {
            if(id < 1) { throw new NullReferenceException("Input Invalid."); }
            var equipmentToDelete = _FindEquipmentRepository.FindBy(x => x.Id == id);
            if (equipmentToDelete == null) { throw new NullReferenceException("Equipment Empty."); }
            _EquipmentRepository.Delete(equipmentToDelete);
            _EquipmentRepository.Save();
        }

    }
}
