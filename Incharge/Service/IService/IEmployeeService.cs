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

        //public void AddClientToEmployee(EmployeeVM employeeVM)
        //{
        //    var employee = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
        //    if (employee == null) { throw new ArgumentNullException("Employee don't exist"); }
        //    if(employeeVM.ClientId == null) { throw new ArgumentNullException("No Client in list"); }
        //    foreach (var clientId in employeeVM.ClientId)
        //    {
        //        var clientToAdd = _FindClientRepository.FindBy(c => c.Id == clientId); //check if this repeats itself
        //        if (clientToAdd == null) { throw new ArgumentNullException("Client don't exist"); }
        //        employee.Clients.Add(clientToAdd);
        //    }
        //    _EmployeeRepository.Update(employee);
        //    _EmployeeRepository.Save();
        //}
        //public void AddGymClassToEmployee(EmployeeVM employeeVM)
        //{
        //    var employee = _FindEmployeeRepository.FindBy(e => e.Uuid == employeeVM.Uuid);
        //    if (employee == null) { throw new ArgumentNullException("Employee don't exist"); }
        //    if (employeeVM.GymclassesId == null) { throw new ArgumentNullException("No Class Entered"); }
        //    foreach (var GymclassesId in employeeVM.GymclassesId)
        //    {
        //        var ClassToAdd = _FindGymClassRepository.FindBy(c => c.Id == GymclassesId); //check if this repeats itself
        //        if (ClassToAdd == null) { throw new ArgumentNullException("Gym Class don't exist"); }
        //        employee.Gymclasses.Add(ClassToAdd);
        //    }
        //    _EmployeeRepository.Update(employee);
        //    _EmployeeRepository.Save();
        //}
    }
}
