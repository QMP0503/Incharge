using Incharge.Models;
using Incharge.DTO;
using Incharge.ViewModels;

namespace Incharge.Service.IService
{
    public interface IEmployeeService
    {
        List<EmployeeVM> ListEmployee();
        EmployeeVM FindEmployee(EmployeeVM employeeVM);
        void AddEmployee(EmployeeVM employeeVM);
        void UpdateEmployee(EmployeeVM employeeVM);
        void DeleteEmployee(string Uuid);
        void AddClientToEmployee(EmployeeVM employeeVM);
        void AddGymClassToEmployee(EmployeeVM employeeVM);
    }
}
