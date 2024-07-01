using Incharge.Models;

namespace Incharge.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public double? TotalPrice { get; set; }

        public int ProductTypeId { get; set; }

        public virtual Producttype ProductType { get; set; }
        
        //used to extract data from view
        public virtual List<int> SalesId { get; set; } = new List<int>();

        public virtual List<int> ClientsId { get; set; } = new List<int>();

        public virtual List<int> DiscountsId { get; set; } = new List<int>();

        //strictly used to present data in view
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

        public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    }
}
