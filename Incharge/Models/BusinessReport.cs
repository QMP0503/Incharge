using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Incharge.Models;

public partial class BusinessReport
{
    public int Id { get; set; }
    public string Uuid { get; set; }
    public DateTime Date { get; set; } //always start at the beginning of the month

    //add option to set the date of the report later
    public int MonthlyMembers { get; set; } //determined by summing clients with active memberships during the month
    public double Revenue { get; set; }
    public double Cost { get; set; }
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public double AccountsPayable { get; set; }
    public double AccountsRecievable { get; set; }
    //income
    public double Profit { get; set; }
    public double MembershipFee { get; set; } //earnings from memberships
    public double NewMembershipSales { get; set; }

    //operating expenses (sorted through by expense type)
    public double Wages { get; set; } //total wages paid to employees
    public double Rent { get; set; }
    public double Utilities { get; set; } //might not even need
    public double Insurance { get; set; }
    public double Maintenance { get; set; } //overall maintance cost
    public double Equipment { get; set; } //for purchase of new equipment
    public double OtherExpenses { get; set; }
}
