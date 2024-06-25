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
        

        //Relationship for other methods like (add gymclass, add product, etc.) adding and updating clients will not use this
        //ALL like will store ID of items so that razor page can access and use.
        public List<int?> SalesID { get; set; } = new List<int?>();

        public List<string> EmployeeID { get; set; } = new List<string>();

        public List<int?> Gymclasses { get; set; } = new List<int?>();

        public List<int?> Products { get; set; } = new List<int?>();

    }
}
