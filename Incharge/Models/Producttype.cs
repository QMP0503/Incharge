
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Producttype
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    [AllowedValues(typeof(string), new string[] {"weekly", "Monthly", "Yearly" })] //make enum
    public string? Recurance { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
