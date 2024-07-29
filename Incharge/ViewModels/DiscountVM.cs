using Incharge.Models;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class DiscountVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double DiscountValue { get; set; } //store in decimal
        public virtual ICollection<Sale>? Sales { get; set; } //discount applied to salse

        //data from view only
        public virtual List<int> SalesId { get; set; }

        //Error view only
        public string? Error { get; set; }
    }
}
