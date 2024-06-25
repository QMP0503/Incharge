using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Service.IService
{
    public interface IEquipmentService
    {
        public List<Equipment> ListEquipment(Func<Equipment, bool> predicate);
        public Equipment FindEquipment(Func<Equipment, bool> predicate);
        public void AddEquipment(EquipmentVM equipmentVM);
        public void UpdateEquipment(EquipmentVM equipmentVM);
        public void DeleteEquipment(int id);
    }
}
