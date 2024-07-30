
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Expense
{
    public int Id { get; set; }
    public string Uuid { get; set; }

    [AllowedValues(typeof(string), new string[] { "Wages", "Rent", "Utilities", "Insurance", "Equipment", "Maintance", "Other" })]
    public string Type { get; set; }
    public DateTime Date { get; set; }

    public string Name { get; set; } = null!;

    public double Cost { get; set; }

    public string? Description { get; set; }

    public int? BusinessReportId { get; set; }

    public virtual BusinessReport? BusinessReport { get; set; }


}
