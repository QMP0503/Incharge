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
            if(entity.FirstName == null || entity.LastName == null || entity.Email == null || entity.Phone == 0 || entity.RoleId == 0) { throw new Exception("A value input is empty. Please try again."); }

            //email and phone checker
            PhoneChecker(entity);
            EmailChecker(entity);

            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == entity.RoleId);
            if (employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }

            var employee = _Mapper.Map<Employee>(entity);

            if (entity.PicutreInput != null)
            {
                var result = _PhotoService.AddPhotoAsync(entity.PicutreInput).Result;
                employee.ProfilePicture = result.Url.ToString();

            }
            else
            {
                employee.ProfilePicture = "https://res.cloudinary.com/dmmlhlebe/image/upload/v1721294595/default_z7dhuq.png";
            }
            ; //maps the entity to employee
            employee.TotalSalary = entity.TotalSalary ?? employeeType.Salary;
            employee.Role = employeeType;
            _EmployeeRepository.Add(employee);
            _EmployeeRepository.Save();
        }
        public void UpdateService(EmployeeVM entity) //add ignore null member mapping configuration
        {
            if (entity == null) { throw new ArgumentNullException("employee empty"); }
            var employeeToUpdate = _FindEmployeeRepository.FindBy(e => e.Uuid == entity.Uuid);
            if (employeeToUpdate == null) { throw new ArgumentNullException("Employee don't exist"); }

            //email and phone checker
            if(entity.Phone != employeeToUpdate.Phone)
            {
                PhoneChecker(entity);
            }
            if(entity.Email != employeeToUpdate.Email)
            {
                EmailChecker(entity);
            }


            var employeeType = _FindEmployeeTypeRepository.FindBy(e => e.Id == entity.RoleId);
            if (employeeType == null) { throw new ArgumentNullException("employee type don't exist"); }
            entity.Role = employeeType;
            var result = entity.PicutreInput != null ? _PhotoService.AddPhotoAsync(entity.PicutreInput).Result : null;
            if (result != null) 
            {
                entity.ProfilePicture = result.Url.ToString();
                if(employeeToUpdate.ProfilePicture != null && employeeToUpdate.ProfilePicture!= "https://res.cloudinary.com/dmmlhlebe/image/upload/v1721294595/default_z7dhuq.png")
                {
                    var delete = _PhotoService.DeletePhotoAsync(employeeToUpdate.ProfilePicture).Result;
                }
            }
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
        public void PhoneChecker(EmployeeVM entity)
        {
            //phone checker
            var clientPhoneCheck = _FindClientRepository.FindBy(x => x.Phone == entity.Phone);
            var employeePhoneCheck = _FindEmployeeRepository.FindBy(x => x.Phone == entity.Phone);
            if (clientPhoneCheck != null || employeePhoneCheck != null) { throw new Exception("Phone number already exist."); }
            
        }
        public void EmailChecker(EmployeeVM entity)
        {
            //email checker
            var clientEmailCheck = _FindClientRepository.FindBy(x => x.Email == entity.Email);
            var employeeEmailCheck = _FindEmployeeRepository.FindBy(x => x.Email == entity.Email);
            if (clientEmailCheck != null || employeeEmailCheck != null) { throw new Exception("Email already exist."); }
        }


    }
}
