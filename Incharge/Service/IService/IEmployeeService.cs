using Incharge.Models;
using Incharge.DTO;
using Incharge.ViewModels;

namespace Incharge.Service.IService
{
    public interface IEmployeeService
    {
        List<EmployeeDTO> ListEmployee();
        EmployeeDTO FindEmployee(EmployeeVM employeeVM);
        void AddEmployee(EmployeeVM employeeVM);
        void UpdateEmployee(EmployeeVM employeeVM);
        void DeleteEmployee(string Uuid);
        void AddClientToEmployee(EmployeeVM employeeVM);
    }
}
