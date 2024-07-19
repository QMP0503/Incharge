using Incharge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Incharge.ViewModels
{
    public class ExpenseVM
    {   
        [ValidateNever]
        public string? Uuid { get; set; }

        [AllowedValues("Wages", "Rent", "Utilities", "Insurance", "Equipment", "Maintance", "Other")]
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
        public List<string> TypeOptions { get; set; } = new List<string>() { "Wages","Rent", "Utilities", "Insurance", "Equipment", "Maintance", "Other" };

        //Filter based on recurrance type?
        public List<string> PaymentTypes { get; set; } = new List<string>() { "Recurring", "One Time" };

        //Error Message
        public string? Error { get; set; }
    }
}
