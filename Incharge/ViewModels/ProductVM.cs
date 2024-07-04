using Incharge.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        [DisplayName("Total Price")]
        public double? TotalPrice { get; set; }

        public int ProductTypeId { get; set; }

        [DisplayName("Product Type")]
        public virtual Producttype ProductType { get; set; }

        //since one product can only have one type this is strictly for selection in add/edit view for product 

        //VIEW ONLY
        public virtual List<Producttype> ProductTypeOption { get; set; } = new List<Producttype>();
        public virtual List<Client> ClientOptions { get; set; } = new List<Client>();
        
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
