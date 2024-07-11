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
        private readonly IPhotoService _PhotoService;
        public EquipmentService(IPhotoService photoService, IMapper mapper, IFindRepository<Equipment> FindEquipmentRepository, IRepository<Equipment> EquipmentRepository)
        {
            _FindEquipmentRepository = FindEquipmentRepository;
            _EquipmentRepository = EquipmentRepository;
            _Mapper = mapper;
            _PhotoService = photoService;
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
            var result = _PhotoService.AddPhotoAsync(equipmentVM.PictureFile).Result;
            if (result == null) { throw new NullReferenceException("Photo Empty or Invalid."); }
            equipmentVM.Image = result.Url.ToString();
            var equipment = _Mapper.Map<Equipment>(equipmentVM);
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

            //delete photo first before new one can be generated
            if(equipment.Image != null)
            {
                var delete = _PhotoService.DeletePhotoAsync(equipment.Image).Result;
                if (delete == null) { throw new NullReferenceException("Photo Empty or Invalid."); }
            }

            if (equipmentVM.PictureFile != null)
            {
                var result = _PhotoService.AddPhotoAsync(equipmentVM.PictureFile).Result;
                if (result == null) { throw new NullReferenceException("Photo Empty or Invalid."); }
                equipmentVM.Image = result.Url.ToString();
            }
            _Mapper.Map(equipmentVM, equipment);
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
