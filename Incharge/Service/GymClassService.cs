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
        readonly IChecker _checker;
        readonly IRepository<Equipment> _EquipmentRepository;
        public GymClassService(IRepository<Equipment> equipmentRepository, IChecker checker, IFindRepository<Equipment> FindEquipmentRepository, IMapper mapper, IFindRepository<Gymclass> FindGymClassRepository, IRepository<Gymclass> GymClassRepository, IFindRepository<Location> locationRepository, IFindRepository<Employee> employeeRepository, IFindRepository<Client> clientRepository, IRepository<Client> ClientRepository)
        {
            _Mapper = mapper;
            _FindGymClassRepository = FindGymClassRepository;
            _GymClassRepository = GymClassRepository;
            _FindLocationRepository = locationRepository;
            _FindEmployeeRepository = employeeRepository;
            _FindClientRepository = clientRepository;
            _FindEquipmentRepository = FindEquipmentRepository;
            _ClientRepository = ClientRepository;
            _checker = checker;
            _EquipmentRepository = equipmentRepository;
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


            if (entity.Type == "Private")
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

            //for changing class status only
            switch (entity.Status)
            {
                case "Active":
                    if(gymClassToUpdate.Status == "Cancelled") //would be a strange case but should have the option
                    {
                        if(entity.Type == "Private")
                        {
                            foreach (var client in gymClassToUpdate.Clients)
                            {
                                client.TotalTrainingSessions -= 1;
                                _ClientRepository.Update(client);
                            }
                        }
                        gymClassToUpdate.Status = "Active";
                        _GymClassRepository.Save();
                        return;
                    }
                    if(gymClassToUpdate.Status == "Completed")
                    {
                        throw new Exception("Cannot activate a completed class. If mistake was made be make new entry for class.");
                    };
                    break;
                case "Cancelled":
                    if (gymClassToUpdate.Status == "Active")
                    {
                        if(entity.Type == "Private")
                        {
                            foreach (var client in gymClassToUpdate.Clients)
                            {
                                client.TotalTrainingSessions += 1;
                                _ClientRepository.Update(client);
                            }
                        }
                        gymClassToUpdate.Status = "Cancelled";
                        _GymClassRepository.Save();
                        return;
                    }
                    if(gymClassToUpdate.Status == "Completed")
                    {
                        throw new Exception("Cannot cancel a completed class.  If mistake was made be make new entry for class.");
                    };
                    break;
                case "Completed":
                    if(gymClassToUpdate.Status == "Cancelled")
                    {
                        throw new Exception("Cannot complete a cancelled class.  If mistake was made be make new entry for class.");
                    };
                    if(gymClassToUpdate.Status == "Active")
                    {
                        gymClassToUpdate.Status = "Complete";
                        _GymClassRepository.Save();
                        return;
                    };
                    break;
                default:
                    break;
            }
            

            //For when class is ACTUALLY editted

            

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
            if (entity.ClientsId != null)
            {
                //reset client training session counter to make edit easier
                foreach (var client in gymClassToUpdate.Clients)
                {
                    client.TotalTrainingSessions += 1;
                    gymClassToUpdate.Clients.Remove(client);
                    _ClientRepository.Update(client);
                }
                foreach (var clientId in entity.ClientsId)
                {
                    var client = _FindClientRepository.FindBy(x => x.Id == clientId);
                    if (client == null) { throw new Exception("Client don't exist."); }
                    if (client.TotalTrainingSessions <= 0) { throw new Exception("Client don't have enough training sessions."); }
                    client.TotalTrainingSessions -= 1;
                    gymClassToUpdate.Clients.Add(client);
                    _ClientRepository.Update(client);
                }
            }
            if (entity.EquipmentId != null)
            {
                //reset equipment list to make things easier
                foreach (var equipment in gymClassToUpdate.Equipment)
                {
                    gymClassToUpdate.Equipment.Remove(equipment);
                }
                foreach (var equipmentId in entity.EquipmentId)
                {
                    var equipment = _FindEquipmentRepository.FindBy(x => x.Id == equipmentId);
                    if(equipment == null) { throw new Exception("Cannot find equipment"); }
                    if(equipment.Status == "Unavailable") { throw new Exception("Equipment is unavailable."); }
                    if(equipment.GymClasses.Any(x => x.Date <= gymClassToUpdate.Date && x.EndTime >= gymClassToUpdate.EndTime)) { throw new Exception("Equipment already registered in class a selected time."); }
                    gymClassToUpdate.Equipment.Add(equipment);
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

            _checker.LocationCheck();
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
                EquipmentOptions = _FindEquipmentRepository.ListBy(x => x.Status=="Available"),
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
