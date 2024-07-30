using Incharge.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Incharge.ViewModels
{
    public class ClientVM
    {
        [AllowNull]
        public string Uuid { get; set; }  //info being fed back from the client
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        //display url link for profile picture
        [AllowNull]
        [DisplayName("Profile Picture")]
        public string ProfilePicture { get; set; }

        public long? Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        //sign in and out of the gym, when payment is overdue they can no longer enter the gym

        [AllowedValues("Signed In", "Signed Out")]
        public string? Status { get; set; }

        [AllowNull]
		[AllowedValues("Active", "Inactive", "Suspended", "Overdue", "No Membership", null)]
		public string? MembershipStatus { get; set; }

		[AllowNull]
		public string? MembershipName { get; set; }
        public int MembershipProductId { get; set; } //for searching purposes
        [DataType(DataType.Date)]
        public DateTime MembershipExpiryDate { get; set; }
		[DataType(DataType.Date)]
		public DateTime MembershipStartDate { get; set; }

        //FOR PT
        public int TotalTrainingSessions { get; set; }

        public string? Note { get; set; }
        public string? Address { get; set; } //address of client


        //Icollection<object> to retrieve and display data.
        public virtual ICollection<Sale>? Sales { get; set; }  //do i need this in dto??
        [AllowNull]
        public virtual ICollection<Employee>? Employees { get; set; }

        //Consider going through DTO pushing into view. Don't need to see everything about gym class
        [AllowNull]
        public virtual ICollection<Gymclass>? Gymclasses { get; set; }
        [AllowNull]
        public virtual ICollection<Product>? Products { get; set; }



        [Display(Name = "Gym Membership")]
		[AllowNull]
		[ValidateNever]
		public Product GymMembership { get; set; }

        //Selected object Id retrieved from view page
        public List<int?> SalesID { get; set; } = new List<int?>();

        public List<string> EmployeeID { get; set; } = new List<string>();

        public List<int?> GymClassID { get; set; } = new List<int?>();

        public List<int?> ProductID { get; set; } = new List<int?>();
        public int? PaymentRecordId { get; set; }

        //INPUT ONLY
        [DisplayName("Add Profile Picture")]
        [AllowedExtensions(new string[]{ ".jpg", ".png", ".jpeg", ".jfif" })]
		public IFormFile? PicutreInput { get; set; }

        //ERROR MESSAGE FOR VIEW ONLY
        public string? Error { get; set; } //error message from server
    }
}
