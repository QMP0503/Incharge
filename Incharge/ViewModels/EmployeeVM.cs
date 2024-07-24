using Incharge.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Incharge.ViewModels
{
    public class EmployeeVM
    {
        public string? Uuid { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;
        public long Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        [DisplayName("Total Salary")]
        public double? TotalSalary { get; set; }
        [DisplayName("Profile Picture")]
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
		[AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        [AllowNull]
		public IFormFile PicutreInput { get; set; }


		//ERROR MESSAGE FOR VIEW ONLY
		public string? Error { get; set; } //error message from server
	}
}
