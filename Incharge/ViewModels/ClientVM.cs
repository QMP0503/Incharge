using Incharge.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Policy;

namespace Incharge.ViewModels
{
    public class ClientVM
    {
        public string Uuid { get; set; }  //info being fed back from the client
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        public int Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        //sign in and out of the gym, when payment is overdue they can no longer enter the gym
        [AllowedValues(typeof(string), new string[] { "SignedIn", "SignedOut", "Overdue" })]
        public string? Status { get; set; }

        //Icollection<object> to retrieve and display data.
        public virtual Paymentrecord? PaymentRecord { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }  //do i need this in dto??
        public virtual ICollection<Employee> Employees { get; set; }
       
        //Consider going through DTO pushing into view. Don't need to see everything about gym class
        public virtual ICollection<Gymclass> Gymclasses { get; set; } 

        public virtual ICollection<Product> Products { get; set; }


        //Selected object Id retrieved from view page
        public List<int?> SalesID { get; set; } = new List<int?>();

        public List<string> EmployeeID { get; set; } = new List<string>();

        public List<int?> GymClassID { get; set; } = new List<int?>();

        public List<int?> ProductID { get; set; } = new List<int?>();

    }
}
