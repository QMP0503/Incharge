using Incharge.Models;
using Incharge.ViewModels;
using Incharge.Service.IService;
using Incharge.Repository.IRepository;
using AutoMapper;

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
        readonly IRepository<Client> _ClientRepository;
        readonly IMapper _Mapper;
        readonly IChecker<LocationVM> _locationStatusChecker;
        public GymClassService(IChecker<LocationVM> locationStatusChecker, IFindRepository<Equipment> FindEquipmentRepository, IMapper mapper, IFindRepository<Gymclass> FindGymClassRepository, IRepository<Gymclass> GymClassRepository, IFindRepository<Location> locationRepository, IFindRepository<Employee> employeeRepository, IFindRepository<Client> clientRepository, IRepository<Client> ClientRepository)
        {
            _Mapper = mapper;
            _FindGymClassRepository = FindGymClassRepository;
            _GymClassRepository = GymClassRepository;
            _FindLocationRepository = locationRepository;
            _FindEmployeeRepository = employeeRepository;
            _FindClientRepository = clientRepository;
            _FindEquipmentRepository = FindEquipmentRepository;
            _ClientRepository = ClientRepository;
            _locationStatusChecker = locationStatusChecker;
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
        public void AddService(GymClassVM entity) //have switch case if feed back require more class types
        {
            //Checking if date is input correctly
            if(entity.Date > entity.EndTime) { throw new Exception("End time must be after start time"); }

            var location = _FindLocationRepository.FindBy(x => x.Id == entity.LocationId);
            var employee = _FindEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
           
            //Checking for element clash
            var checkTimeAndLocation = _FindGymClassRepository.FindBy(x =>( (x.EndTime <= entity.EndTime && x.EndTime >= entity.Date)||(x.Date <= entity.EndTime && x.Date >= entity.Date))&& x.LocationId == entity.LocationId);
            if (checkTimeAndLocation != null) { throw new Exception("A class already exist within time frame and in the same location."); }
            var checkTimeAndEmployee = _FindGymClassRepository.FindBy(x =>( (x.EndTime <= entity.EndTime && x.EndTime >= entity.Date) || (x.Date <= entity.EndTime && x.Date >= entity.Date)) && x.EmployeeId == entity.EmployeeId);
            if(checkTimeAndEmployee != null) { throw new Exception("A class already exist within time frame and with the same employee."); }

            //Start Mapping
            var gymClass = _Mapper.Map<Gymclass>(entity);
            gymClass.Status = "Active";
            gymClass.Location = location;
            gymClass.Employee = employee;

            if(entity.EquipmentId != null)
            {
                foreach (var equipmentId in entity.EquipmentId)
                {
                    var equipment = _FindEquipmentRepository.FindBy(x => x.Id == equipmentId);
                    equipment.Status = "Reserved";
                    gymClass.Equipment.Add(equipment);
                }
            }

            if(entity.Type == "Private")
            {
                if(entity.ClientsId == null) { throw new Exception("Private class must have a client"); }
                foreach(var clientId in entity.ClientsId)
                {
                    var client = _FindClientRepository.FindBy(x => x.Id == clientId);
                    if (client == null) { throw new Exception("Client don't exist."); }
                    if(client.TotalTrainingSessions <= 0) { throw new Exception("Client don't have enough training sessions."); }
                    client.TotalTrainingSessions -= 1;
                    gymClass.Clients.Add(client);
                    _ClientRepository.Update(client);
                }
            }
            _GymClassRepository.Add(gymClass);
            _GymClassRepository.Save();
        }
        public void UpdateService(GymClassVM entity)
        {          
            var gymClassToUpdate = _FindGymClassRepository.FindBy(x => x.Id == entity.Id);
            if(gymClassToUpdate == null) { throw new Exception("GymClass don't exist."); }

            //for private training for client have refund options
            if (gymClassToUpdate.Type == "Private" && entity.Status == "Cancelled" && gymClassToUpdate.Status == "Active")
            {
                foreach (var client in gymClassToUpdate.Clients)
                {
                    client.TotalTrainingSessions += 1;
                    _ClientRepository.Update(client);
                }
            }
            if (gymClassToUpdate.Type == "Private" && entity.Status == "Active" && gymClassToUpdate.Status == "Cancelled")
            {
                foreach (var client in gymClassToUpdate.Clients)
                {
                    client.TotalTrainingSessions -= 1;
                    _ClientRepository.Update(client);
                }
            }

            _Mapper.Map(entity, gymClassToUpdate);

            if(entity.LocationId != 0)
            {
                //checking for clash
                var checkTimeAndLocation = _FindGymClassRepository.FindBy(x => (x.EndTime <= entity.EndTime && x.EndTime >= entity.Date)&&(x.Date <= entity.EndTime && x.Date >= entity.Date) && x.LocationId == entity.LocationId);
                if (checkTimeAndLocation != null && checkTimeAndLocation.Id != entity.Id) { throw new Exception("A class already exist within time frame and in the same location."); }

                var location = _FindLocationRepository.FindBy(x => x.Id == entity.LocationId);
                gymClassToUpdate.Location = location;
            }
            if(entity.EmployeeId != 0)
            {
                //checking for clash
                var checkTimeAndEmployee = _FindGymClassRepository.FindBy(x => (x.EndTime <= entity.EndTime && x.EndTime >= entity.Date) &&(x.Date <= entity.EndTime && x.Date >= entity.Date) && x.EmployeeId == entity.EmployeeId);
                if (checkTimeAndEmployee != null && checkTimeAndEmployee.Id != entity.Id) { throw new Exception("A class already exist within time frame and with the same employee."); }
                var employee = _FindEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
                gymClassToUpdate.Employee = employee;
            }
            if(entity.ClientsId != null)
            {
                foreach (var clientId in entity.ClientsId)
                {
                    var client = _FindClientRepository.FindBy(x => x.Id == clientId);
                    if(client == null) { throw new Exception("Cannot find clients"); }
                    gymClassToUpdate.Clients.Add(client);
                }
            }

            

            if (gymClassToUpdate.Employee != null)
            {
                gymClassToUpdate.EmployeeId = gymClassToUpdate.Employee.Id;
            }
            if(gymClassToUpdate.Location != null)
            {
                gymClassToUpdate.LocationId = gymClassToUpdate.Location.Id;
            }

            _locationStatusChecker.Check();

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
            var entity = new GymClassVM()
            {
                LocationOptions = _FindLocationRepository.ListBy(x => true),
                EmployeeOptions = _FindEmployeeRepository.ListBy(x => x.Role.Type == "Trainer"),
                EquipmentOptions = _FindEquipmentRepository.ListBy(x => true),
                ClientOptions = _FindClientRepository.ListBy(x => true),
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
