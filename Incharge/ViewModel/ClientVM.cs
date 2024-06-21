using Incharge.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModel
{
    public class ClientVM
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        //sign in and out of the gym, when payment is overdue they can no longer enter the gym
        [AllowedValues(typeof(string), new string[] { "SignedIn", "SignedOut", "Overdue" })]
        public string? Status { get; set; }
        [ForeignKey("PaymentRecordId")]
        public int? PaymentRecordId { get; set; }


        //All below figure out when designing client controller pages.
        public virtual Paymentrecord? PaymentRecord { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public virtual ICollection<Gymclass> Gymclasses { get; set; } = new List<Gymclass>();

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
