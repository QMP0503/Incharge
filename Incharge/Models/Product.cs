
namespace Incharge.Models;

public partial class Product
{ 
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double TotalPrice { get; set; }
    
    public int ProductTypeId { get; set; }

    public virtual Producttype ProductType { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();


}
