using Incharge.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class ExpenseVM
    {
        public string Uuid { get; set; }

        [AllowedValues(typeof(string), new string[] {"Wages, Rent, Utilities", "Insurance", "Eqipment", "Maintance", "Other" })]
        public string Type { get; set; }

        public DateTime Date { get; set; } = DateTime.Now; 
        //Default assumption that the date expense was made is the current date
        //Date for when the expense was made

        public string Name { get; set; } = null!;

        public double Cost { get; set; }

        public string? Description { get; set; }

        //gather information from view model
        public int? BusinessReportId { get; set; }

        //assign relationship
        public virtual BusinessReport? BusinessReport { get; set; }

        //VIEW ONLY TO SELECT:
        public List<string> TypeOptions { get; set; } = new List<string>() { "Wages, Rent, Utilities", "Insurance", "Eqipment", "Maintance", "Other" };
    }
}
