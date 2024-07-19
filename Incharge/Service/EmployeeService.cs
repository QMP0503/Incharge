using AutoMapper;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.ViewModels;
namespace Incharge.Service
{
    public class EmployeeService : IService<EmployeeVM, Employee>, IDropDownOptions<EmployeeVM>
    {
        readonly IFindRepository<Employee> _FindEmployeeRepository;
        readonly IRepository<Employee> _EmployeeRepository;
        readonly IFindRepository<EmployeeType> _FindEmployeeTypeRepository;
        readonly IFindRepository<Client> _FindClientRepository;
        readonly IFindRepository<Gymclass> _FindGymClassRepository;
        readonly IMapper _Mapper;
        readonly IPhotoService _PhotoService;

        public EmployeeService(IPhotoService photoService, IFindRepository<Client> FindClientRepository, IFindRepository<Employee> FindEmployeeRepository, IRepository<Employee> EmployeeRepository, IMapper Mapper, IFindRepository<EmployeeType> findEmployeeTypeRepository, IFindRepository<Gymclass> findGymClassRepository)
        {
            _FindEmployeeRepository = FindEmployeeRepository;
            _EmployeeRepository = EmployeeRepository;
            _Mapper = Mapper;
            _FindEmployeeTypeRepository = findEmployeeTypeRepository;
            _FindClientRepository = FindClientRepository;
            _FindGymClassRepository = findGymClassRepository;
            _PhotoService = photoService;
        }
        public List<EmployeeVM> ListItem(Func<Employee, bool> predicate) //recosider if i need this.
        {
            var employeeList = _FindEmployeeRepository.ListBy(predicate);
            var employeeVMLIst = _Mapper.Map<List<EmployeeVM>>(employeeList);
            return employeeVMLIst;
        }
        public EmployeeVM GetItem(Func<Employee, bool> predicate)
        {
            var employee = _FindEmployeeRepository.FindBy(predicate);
            var entity = _Mapper.Map<EmployeeVM>(employee);
            entity.EmployeeTypeOptions = DropDownOptions().EmployeeTypeOptions;
            return entity;
        }
        public void AddService(EmployeeVM entity)//employee type
        {
            if (entity == null) { throw new ArgumentNullException("employee empty"); }
            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == entity.RoleId);
            if (employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }
            var result = _PhotoService.AddPhotoAsync(entity.PicutreInput).Result;
            entity.ProfilePicture = result.Url.ToString();
            var employee = _Mapper.Map<Employee>(entity); //maps the entity to employee
            employee.TotalSalary = entity.TotalSalary ?? employeeType.Salary;
            _EmployeeRepository.Add(employee);
            _EmployeeRepository.Save();
        }
        public void UpdateService(EmployeeVM entity) //add ignore null member mapping configuration
        {
            if (entity == null) { throw new ArgumentNullException("employee empty"); }
            var employeeToUpdate = _FindEmployeeRepository.FindBy(e => e.Uuid == entity.Uuid);
            if (employeeToUpdate == null) { throw new ArgumentNullException("Employee don't exist"); }
            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == entity.RoleId);
            if (employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }
            entity.Role = employeeType;
            var result = entity.PicutreInput != null ? _PhotoService.AddPhotoAsync(entity.PicutreInput).Result : null;
            if (result != null) { entity.ProfilePicture = result.Url.ToString(); }
            var delete = _PhotoService.DeletePhotoAsync(employeeToUpdate.ProfilePicture).Result;
            var phoneNum = employeeToUpdate.Phone;
            _Mapper.Map(entity, employeeToUpdate); //maps the entity to employee
            if(employeeToUpdate.Phone == 0) { employeeToUpdate.Phone = phoneNum; }
            //incase mapper maps 0
            employeeToUpdate.TotalSalary = entity.TotalSalary ?? employeeToUpdate.Role.Salary;
            _EmployeeRepository.Update(employeeToUpdate);
            _EmployeeRepository.Save();
        }
        public void DeleteService(EmployeeVM entity)
        {
            var employeeToDelete = _FindEmployeeRepository.FindBy(e => e.Uuid == entity.Uuid);
            if (employeeToDelete == null) { throw new ArgumentNullException("Employee don't exist"); }
            var delete = _PhotoService.DeletePhotoAsync(employeeToDelete.ProfilePicture).Result;
            _EmployeeRepository.Delete(employeeToDelete);
            _EmployeeRepository.Save();
        }

        public EmployeeVM DropDownOptions()
        {
            var entity = new EmployeeVM()
            {
                EmployeeTypeOptions = _FindEmployeeTypeRepository.ListBy(x => true)
            };
            return entity;
        }


    }
}
