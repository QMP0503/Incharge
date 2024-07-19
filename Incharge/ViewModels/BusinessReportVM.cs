using Incharge.Models;
using Org.BouncyCastle.Asn1.Mozilla;

namespace Incharge.ViewModels
{
    public class BusinessReportVM
    {
        public string Uuid { get; set; }
        public DateTime Date { get; set; }

        public double? Revenue { get; set; }

        public int MonthlyMembers { get; set; }

        public double? Cost { get; set; }
        
        //Collecting data from vm... (might not even need ngl)
        public List<string> ExpensesUuid { get; set; }
        public List<string> SalesUuid { get; set; }
        //View ONLY
        public virtual ICollection<Expense>? Expenses { get; set; }
        public virtual ICollection<Sale>? Sales { get; set; }


        //Financial data calculated when program boot, not stored
        public double AccountsPayable {  get; set; }
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
        public double Equipment { get; set; }
        public double Mantaince { get; set; }
        public double OtherExpenses { get; set; }

        //Error Message
        public string? Error { get; set; }
        
    }
}
