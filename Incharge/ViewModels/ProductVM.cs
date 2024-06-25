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

        public virtual List<int> SalesId { get; set; } = new List<int>();

        public virtual List<int> ClientsId { get; set; } = new List<int>();

        public virtual List<int> DiscountsId { get; set; } = new List<int>();
    }
}
