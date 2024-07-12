using Incharge.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class EmployeeVM
    {
        public string? Uuid { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        public string Address { get; set; }
        public double? TotalSalary { get; set; }
        public string? ProfilePicture { get; set; } //get cloudinary working for image upload


        //object list to display and munipulated (currently set as nullabkle because we have no data)
        public virtual ICollection<Gymclass>? Gymclasses { get; set; }
        public virtual EmployeeType? Role { get; set; }
        public virtual ICollection<Sale>? Sales { get; set; } 
        public virtual ICollection<Client>? Clients { get; set; } 
        
        //VIEW ONLY
        public List<EmployeeType>? EmployeeTypeOptions { get; set; }


        //INPUT ONLY
        public virtual List<int>? GymclassesId { get; set; }
        public int RoleId { get; set; }
        public virtual List<int>? SalesId { get; set; }
        public virtual List<int>? ClientId { get; set; } //check if this is a security risk to display client int id.

        [DisplayName("Add Profile Picture")]
        public IFormFile PicutreInput { get; set; }
    }
}
