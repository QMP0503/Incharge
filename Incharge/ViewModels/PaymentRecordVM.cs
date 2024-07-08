using Incharge.Models;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class PaymentRecordVM
    {
        public string Uuid { get; set; }
        [AllowedValues(typeof(string), new string[] {"Paided, Pending, Overdue"})]
        public bool? Paymentstatus { get; set; } //find the membership payment to determine if membership is still valid
        public string? Description { get; set; }
        public int? ClientId { get; set; }
        public virtual Client Clients { get; set; } //each client will have a personal payment record
        public virtual ICollection<Sale> ClientSales { get; set; } //list of pruchases clients made
    }
}
