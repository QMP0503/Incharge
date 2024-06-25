using AutoMapper;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.DTO;
using Incharge.ViewModels;
namespace Incharge.Service
{
    public class EmployeeService:IEmployeeService
    {
        readonly IFindRepository<Employee> _FindEmployeeRepository;
        readonly IRepository<Employee> _EmployeeRepository;
        readonly IFindRepository<EmployeeType> _FindEmployeeTypeRepository;
        readonly IMapper _Mapper;

        public EmployeeService(IFindRepository<Employee> FindEmployeeRepository, IRepository<Employee> EmployeeRepository, IMapper Mapper, IFindRepository<EmployeeType> findEmployeeTypeRepository)
        {
            _FindEmployeeRepository = FindEmployeeRepository;
            _EmployeeRepository = EmployeeRepository;
            _Mapper = Mapper;
            _FindEmployeeTypeRepository = findEmployeeTypeRepository;
        }
        public List<EmployeeDTO> ListEmployee() //recosider if i need this.
        {
            var employee = _FindEmployeeRepository.ListBy(x => true);
            var employeeDTO = _Mapper.Map<List<EmployeeDTO>>(employee);
            return employeeDTO;
        }
        public EmployeeDTO FindEmployee(EmployeeVM employeeVM)
        {
            var employee = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
            var employeeDTO = _Mapper.Map<EmployeeDTO>(employee);
            return employeeDTO;
        }
        public void AddEmployee(EmployeeVM employeeVM)//employee type
        {
            if(employeeVM == null) { throw new ArgumentNullException("employee empty"); }
            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == employeeVM.RoleId);
            if(employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }
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
        public void UpdateEmployee(EmployeeVM employeeVM)
        {
            if (employeeVM == null) { throw new ArgumentNullException("employee empty"); }
            var employeeToUpdate = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
            if(employeeToUpdate == null) { throw new ArgumentNullException("Employee don't exist"); }
            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == employeeVM.RoleId);
            if (employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }
            employeeToUpdate.FirstName = employeeVM.FirstName;
            employeeToUpdate.LastName = employeeVM.LastName;
            employeeToUpdate.Email= employeeVM.Email;
            employeeToUpdate.Phone = employeeVM.Phone;
            employeeToUpdate.Role = employeeType;
            _EmployeeRepository.Update(employeeToUpdate);
            _EmployeeRepository.Save();
        }
        public void DeleteEmployee(string Uuid)
        {
            var employeeToDelete = _FindEmployeeRepository.FindBy(e => e.Uuid == Uuid);
            if(employeeToDelete == null) { throw new ArgumentNullException("Employee don't exist"); }
            _EmployeeRepository.Delete(employeeToDelete);
            _EmployeeRepository.Save();
        }
        public void AddClientToEmployee(EmployeeVM employeeVM)
        {

        }
    }
}
