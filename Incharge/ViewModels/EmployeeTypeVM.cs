using Incharge.Models;

namespace Incharge.ViewModels
{
    public class EmployeeTypeVM
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public double? Salary { get; set; }

        //data retrieved from view
        public List<int>? EmployeeIds { get; set; }

        //data stored for display
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
