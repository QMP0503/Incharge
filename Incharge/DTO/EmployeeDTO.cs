using Incharge.Models;
using System.ComponentModel.DataAnnotations;

namespace Incharge.DTO
{
    public class EmployeeDTO
    {
        public string? Uuid { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        public double? TotalSalary { get; set; }
        public virtual ICollection<Gymclass> Gymclasses { get; set; } = new List<Gymclass>();
        public virtual EmployeeType Role { get; set; } = null!;
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
