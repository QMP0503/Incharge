using AutoMapper;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.DTO;
using Incharge.ViewModels;
using AutoMapper.Configuration.Annotations;
namespace Incharge.Service
{
    public class EmployeeService : IEmployeeService, IDropDownOptions<EmployeeVM>
    {
        readonly IFindRepository<Employee> _FindEmployeeRepository;
        readonly IRepository<Employee> _EmployeeRepository;
        readonly IFindRepository<EmployeeType> _FindEmployeeTypeRepository;
        readonly IFindRepository<Client> _FindClientRepository;
        readonly IFindRepository<Gymclass> _FindGymClassRepository;
        readonly IMapper _Mapper;

        public EmployeeService( IFindRepository<Client> FindClientRepository, IFindRepository<Employee> FindEmployeeRepository, IRepository<Employee> EmployeeRepository, IMapper Mapper, IFindRepository<EmployeeType> findEmployeeTypeRepository, IFindRepository<Gymclass> findGymClassRepository)
        {
            _FindEmployeeRepository = FindEmployeeRepository;
            _EmployeeRepository = EmployeeRepository;
            _Mapper = Mapper;
            _FindEmployeeTypeRepository = findEmployeeTypeRepository;
            _FindClientRepository = FindClientRepository;
            _FindGymClassRepository = findGymClassRepository;
        }
        public List<EmployeeVM> ListEmployee() //recosider if i need this.
        {
            var employee = _FindEmployeeRepository.ListBy(x => true);
            var employeeDTO = _Mapper.Map<List<EmployeeVM>>(employee);
            return employeeDTO;
        }
        public EmployeeVM FindEmployee(EmployeeVM employeeVM)
        {
            var employee = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
            var employeeFound = _Mapper.Map<EmployeeVM>(employee);
            return employeeFound;
        }
        public void AddEmployee(EmployeeVM employeeVM)//employee type
        {
            if (employeeVM == null) { throw new ArgumentNullException("employee empty"); }
            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == employeeVM.RoleId);
            if (employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }
            var employee = new Employee
            {
                FirstName = employeeVM.FirstName,
                LastName = employeeVM.LastName,
                Email = employeeVM.Email,
                Phone = employeeVM.Phone,
                Role = employeeType,
            };
            _EmployeeRepository.Add(employee);
            _EmployeeRepository.Save();
        }
        public void UpdateEmployee(EmployeeVM employeeVM) //add ignore null member mapping configuration
        {
            if (employeeVM == null) { throw new ArgumentNullException("employee empty"); }
            var employeeToUpdate = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
            if (employeeToUpdate == null) { throw new ArgumentNullException("Employee don't exist"); }
            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == employeeVM.RoleId);
            if (employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }
            employeeVM.Role = employeeType;
         
            var updatedEmployee = _Mapper.Map<Employee>(employeeVM); //maps the employeeVM to employee
            _EmployeeRepository.Update(updatedEmployee);
            _EmployeeRepository.Save();
        }
        public void DeleteEmployee(string Uuid)
        {
            var employeeToDelete = _FindEmployeeRepository.FindBy(e => e.Uuid == Uuid);
            if (employeeToDelete == null) { throw new ArgumentNullException("Employee don't exist"); }
            _EmployeeRepository.Delete(employeeToDelete);
            _EmployeeRepository.Save();
        }
        public void AddClientToEmployee(EmployeeVM employeeVM)
        {
            var employee = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
            if (employee == null) { throw new ArgumentNullException("Employee don't exist"); }
            if(employeeVM.ClientId == null) { throw new ArgumentNullException("No Client in list"); }
            foreach (var clientId in employeeVM.ClientId)
            {
                var clientToAdd = _FindClientRepository.FindBy(c => c.Id == clientId); //check if this repeats itself
                if (clientToAdd == null) { throw new ArgumentNullException("Client don't exist"); }
                employee.Clients.Add(clientToAdd);
            }
            _EmployeeRepository.Update(employee);
            _EmployeeRepository.Save();
        }
        public void AddGymClassToEmployee(EmployeeVM employeeVM)
        {
            var employee = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
            if (employee == null) { throw new ArgumentNullException("Employee don't exist"); }
            if (employeeVM.GymclassesId == null) { throw new ArgumentNullException("No Class Entered"); }
            foreach (var GymclassesId in employeeVM.GymclassesId)
            {
                var ClassToAdd = _FindGymClassRepository.FindBy(c => c.Id == GymclassesId); //check if this repeats itself
                if (ClassToAdd == null) { throw new ArgumentNullException("Gym Class don't exist"); }
                employee.Gymclasses.Add(ClassToAdd);
            }
            _EmployeeRepository.Update(employee);
            _EmployeeRepository.Save();
        }
        public EmployeeVM DropDownOptions()
        {
            var employeeVM = new EmployeeVM()
            {
                EmployeeTypeOptions = _FindEmployeeTypeRepository.ListBy(x => true)
            };
            return employeeVM;
        }

    }
}
