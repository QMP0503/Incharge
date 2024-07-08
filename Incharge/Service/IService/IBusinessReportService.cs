using Incharge.Models;
using Incharge.ViewModels;


namespace Incharge.Service.IService
{
    public interface IBusinessReportService
    {
        public List<BusinessReportVM> ListItem(Func<BusinessReport, bool> predicate); //test if automapper work with lists
        public BusinessReportVM GetItem(Func<BusinessReport, bool> predicate);
        public void AddService(); //called at the start of the program to create a new business report every month.
        public void UpdateService(); //should be called after every sale and expense entered.
        public void DeleteService(BusinessReportVM entity);
        public BusinessReport CalculateRevenue(BusinessReport entity); 
        public BusinessReport CalculateCost(BusinessReport entity);
        public BusinessReport CalculateProfit(BusinessReport entity);
        public BusinessReport CalculateTotalMembers(BusinessReport entity);//for the month
        public BusinessReport CalculateAccountsReceivable(BusinessReport entity); //Money owed to business by client at the end of the month
        public BusinessReport CalculateAccountsPayable(BusinessReport entity); 
        public BusinessReport CalculateExpenses(BusinessReport entity);
        //Money business owe to supplier and service provider at the end of the month\
        //Might not need cause dont think gyms have any...
        
    }
}
