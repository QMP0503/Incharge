using Incharge.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace Incharge.ViewModels
{
    public class SaleVM 
    {
        public string Uuid { get; set; }  //relationship will still use regular id.
        public DateTime Date { get; set; }

        [AllowedValues("Cash", "Credit", "Debit")]
        public string? PaymentType { get; set; }

        //Makes retrieving data from views easier.
        public int ProductId { get; set; }

        //ADD PRICE WHEN TAX IS ADDED
        public double TotalPrice { get; set; }

        public int Quantity { get; set; }

        //Used for internal calculations and relationship assignment.
        public int EmployeeId { get; set; }

        public int ClientId { get; set; }

        //View page will only show these values.
        public string ClientUuid { get; set; }
        public string EmployeeUuid { get; set; }

        public int? BusinessReportId { get; set; }
        //send to business report within the month, will be made during the background.

        [DisplayName("Business Report")]
        public virtual BusinessReport? BusinessReport { get; set; }

        public virtual Client? Client { get; set; }

        public virtual Employee? Employee { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<Discount>? Discount { get; set; } = new List<Discount>();

        public virtual List<int>? DiscountId { get; set; } 

        //for view only
        [DisplayName("Client Names")]
        public List<Client>? ClientOptions { get; set; }
        [DisplayName("Payment Options")]
        public List<string> PaymentOptions { get; set; } = new List<string>() { "Cash", "Credit", "Debit" };
        public List<Product>? ProductOptions { get; set; }
        [DisplayName("Employee Names")]
        public List<Employee>? EmployeeOptions { get; set; }

        [DisplayName("Product Name")] 
        public string ProductName { get; set; }

        [DisplayName("Discount Options")]
        public List<Discount>? DiscountOptions { get; set; }

        //Error Message
        public string? Error { get; set; }
    }
}
