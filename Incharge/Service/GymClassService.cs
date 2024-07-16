using Incharge.Models;
using Incharge.ViewModels;
using Incharge.Service.IService;
using Incharge.Data;
using Incharge.Repository.IRepository;
using AutoMapper;
using Incharge.ViewModels.Calendar;


namespace Incharge.Service
{
    public class GymClassService:IService<GymClassVM, Gymclass>, IDropDownOptions<GymClassVM>
    {
        readonly IFindRepository<Gymclass> _FindGymClassRepository;
        readonly IFindRepository<Location> _FindLocationRepository;
        readonly IFindRepository<Employee> _FindEmployeeRepository;
        readonly IFindRepository<Equipment> _FindEquipmentRepository;
        readonly IFindRepository<Client> _FindClientRepository;
        readonly IRepository<Gymclass> _GymClassRepository;
        readonly IMapper _Mapper;
        public GymClassService(IFindRepository<Equipment> FindEquipmentRepository, IMapper mapper, IFindRepository<Gymclass> FindGymClassRepository, IRepository<Gymclass> GymClassRepository, IFindRepository<Location> locationRepository, IFindRepository<Employee> employeeRepository, IFindRepository<Client> clientRepository)
        {
            _Mapper = mapper;
            _FindGymClassRepository = FindGymClassRepository;
            _GymClassRepository = GymClassRepository;
            _FindLocationRepository = locationRepository;
            _FindEmployeeRepository = employeeRepository;
            _FindClientRepository = clientRepository;
            _FindEquipmentRepository = FindEquipmentRepository;
        }
        public List<GymClassVM> ListItem(Func<Gymclass, bool> predicate) //test if automapper work with lists
        {
            var gymClassList = _FindGymClassRepository.ListBy(predicate);
            var gymClassVMList = _Mapper.Map<List<GymClassVM>>(gymClassList);
            return gymClassVMList;
        }
        public GymClassVM GetItem(Func<Gymclass, bool> predicate)
        {
            var gymClass = _FindGymClassRepository.FindBy(predicate);
            if (gymClass == null) { throw new ArgumentNullException("GymClass don't exist"); }
            var gymClassVM = _Mapper.Map<GymClassVM>(gymClass);
            return gymClassVM;
        }
        public void AddService(GymClassVM entity)
        {
            var checkGymClass = _FindGymClassRepository.FindBy(x => x.Name == entity.Name);
            if(checkGymClass != null) { throw new Exception("GymClass already exist."); }
            var gymClass = _Mapper.Map<Gymclass>(entity);
            gymClass.Status = "Active";
            //Adding Existing data from other tables
            var location = _FindLocationRepository.FindBy(x => x.Id == entity.LocationId);
            gymClass.Location = location;
            var employee = _FindEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
            gymClass.Employee = employee;

            foreach (var equipmentId in entity.EquipmentId)
            {
                var equipment = _FindEquipmentRepository.FindBy(x => x.Id == equipmentId);
                gymClass.Equipment.Add(equipment);
            }

            _GymClassRepository.Add(gymClass);
            _GymClassRepository.Save();
        }
        public void UpdateService(GymClassVM entity)
        {      
            var gymClassToUpdate = _FindGymClassRepository.FindBy(x => x.Id == entity.Id);
            if(gymClassToUpdate == null) { throw new Exception("GymClass don't exist."); }

            _Mapper.Map(entity, gymClassToUpdate);
            var location = _FindLocationRepository.FindBy(x => x.Id == entity.LocationId);
            gymClassToUpdate.Location = location;
            var employee = _FindEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
            gymClassToUpdate.Employee = employee;
            //foreach(var clientId in entity.ClientsId)
            //{
            //    var client = _FindClientRepository.FindBy(x => x.Id == clientId);
            //    gymClassToUpdate.Clients.Add(client);
            //}
            _GymClassRepository.Update(gymClassToUpdate);
            _GymClassRepository.Save();
        }
        public void DeleteService(GymClassVM entity)
        {
            if(entity ==null) { throw new ArgumentNullException("Empty entry"); }
            var gymClassToDelete = _FindGymClassRepository.FindBy(x => x.Id == entity.Id);
            if(gymClassToDelete == null) { throw new Exception("GymClass don't exist."); }
            _GymClassRepository.Delete(gymClassToDelete);
            _GymClassRepository.Save();
        }

        public GymClassVM DropDownOptions()
        {
            var employeeList = _FindEmployeeRepository.ListBy(x => true);
           // var trainerList = employeeList.Where(x => x.EmployeeType.Name == "Trainer").ToList();
            var entity = new GymClassVM()
            {
                LocationOptions = _FindLocationRepository.ListBy(x => true),
                EmployeeOptions = _FindEmployeeRepository.ListBy(x => true),
                EquipmentOptions = _FindEquipmentRepository.ListBy(x => true),
                TimeSlots = new List<TimeSpan>()
            };
            for (int i = 7; i < 21; i++)
            {
                var time = TimeSpan.FromHours(i);
                entity.TimeSlots.Add(time);
            }
            return entity;
        }
    }
}
