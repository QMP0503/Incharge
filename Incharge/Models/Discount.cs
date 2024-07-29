

namespace Incharge.Models;

public partial class Discount
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double DiscountValue { get; set; } //store in decimal
    public virtual ICollection<Sale>? Sales { get; set; } //discount applied to salse
}
