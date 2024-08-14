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
        public List<EmployeeVM>? EmployeeList { get; set; } //for wage payments only
        public List<ExpenseVM>? RecurringList { get; set; } //Use for utilities and insurance

        //Input for wage only
        public List<string>? EmployeeUuids { get; set; }
        public List<string>? RecurringListUuids { get; set; } //Use for utilities and insurance

        //Error Message
        public string? Error { get; set; }


    }
}
