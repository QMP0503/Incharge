using Incharge.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Incharge.ViewModels
{
    public class SaleVM
    {
        public string Uuid { get; set; } = new Guid().ToString(); //relationship will still use regular id.
        public DateTime Date { get; set; }

        [AllowedValues(typeof(string), new string[] { "Cash", "Credit", "Debit" })]
        public string? PaymentType { get; set; }

        //Makes retrieving data from views easier.
        public int ProductId { get; set; }

        //Used for internal calculations and relationship assignment.
        public int EmployeeId { get; set; }

        public int ClientId { get; set; }

        //View page will only show these values.
        public string ClientUuid { get; set; }
        public string EmployeeUuid { get; set; }

        public int? BusinessReportId { get; set; } //send to business report within the month, will be made during the background.

        public virtual BusinessReport? BusinessReport { get; set; }

        public virtual Client? Client { get; set; }

        public virtual Employee? Employee { get; set; }

        public virtual Product Product { get; set; } = null!;

        //for view only
        public List<Client>? ClientOptions { get; set; } 
        public List<string> PaymentOptions { get; set; } = new List<string>() { "Cash", "Credit", "Debit" };


    }
}
