using Incharge.Models;
using Incharge.ViewModels;
using Incharge.Service.IService;
using AutoMapper;
using Incharge.Repository.IRepository;
using CloudinaryDotNet.Core;

namespace Incharge.Service
{
    //implement business report caching (will be called very often)
    public class BusinessReportService : IBusinessReportService //have more special functions here
    {
        private readonly IFindRepository<Client> _FindClientRepository;
        private readonly IRepository<BusinessReport> _BusinessReportRepository;
        private readonly IFindRepository<Employee> _FindEmployeeRepository;
        private readonly IFindRepository<Gymclass> _FindGymClassRepository;
        private readonly IFindRepository<Product> _FindProductRepository;
        private readonly IFindRepository<Sale> _FindSaleRepository;
        private readonly IFindRepository<BusinessReport> _FindBusinessReportRepository;
        private readonly IFindRepository<Expense> _FindExpenseRepository;
        private readonly IMapper _mapper;
        public BusinessReportService(IFindRepository<BusinessReport> findBusinessReportRepository ,IFindRepository<Product> findProductRepository, IFindRepository<Sale> findSaleRepository, IFindRepository<Gymclass> findGymClassRepository, IFindRepository<Employee> findEmployeeRepository, IFindRepository<Client> FindClientRepository, IFindRepository<Expense> findExpenseRepository, IRepository<BusinessReport> BusinessReportRepository, IMapper mapper)
        {
            _BusinessReportRepository = BusinessReportRepository;
            _FindClientRepository = FindClientRepository;
            _mapper = mapper;
            _FindEmployeeRepository = findEmployeeRepository;
            _FindGymClassRepository = findGymClassRepository;
            _FindProductRepository = findProductRepository;
            _FindSaleRepository = findSaleRepository;
            _FindBusinessReportRepository = findBusinessReportRepository;
            _FindExpenseRepository = findExpenseRepository;
        }
        public List<BusinessReportVM> ListItem(Func<BusinessReport, bool> predicate)
        {
            var businessReportList = _FindBusinessReportRepository.ListBy(predicate);
            var businessReportVMList = _mapper.Map<List<BusinessReportVM>>(businessReportList);
            return businessReportVMList;
        }
        public BusinessReportVM GetItem(Func<BusinessReport, bool> predicate)
        {
            var businessReport = _FindBusinessReportRepository.FindBy(predicate);
            var businessReportVM = _mapper.Map<BusinessReportVM>(businessReport);
            return businessReportVM;
        }
        public void AddService() 
            //assuming that the program is used every day it will automatically create when launched
            //creating new blank business report every month
        {
            var date = DateTime.Now;
            var recentReport = _FindBusinessReportRepository.FindBy(x => x.Date.Month == date.Month && x.Date.Year == date.Year);
            //if given client ability to set date
            //var date = entity.Date; || var date = _BusinessReportRepository.FindBy(x => x.First()).Date;

            if(recentReport == null) 
                //create new business report at beginning of every month
                //Give client ability to set when business report is created (only entry in parameter is date)
            {
                var businessReport = new BusinessReport()
                {
                    Date = date
                };
                _BusinessReportRepository.Add(businessReport);
                _BusinessReportRepository.Save();
            }
        }
        public void UpdateService() //included in sales everytime one is made
        {
            var currentReport = _FindBusinessReportRepository.FindBy(x => x.Date.Month == DateTime.Now.Month);
            if(currentReport == null) { throw new Exception("Report don't exist."); } //Should be very rare, but just in case

            currentReport = CalculateAccountsPayable(currentReport);
            //currentReport = CalculateAccountsReceivable(currentReport); don't have a use for it yet.
            currentReport = CalculateCost(currentReport);
            currentReport = CalculateProfit(currentReport);
            currentReport = CalculateMembershipData(currentReport);
            currentReport = CalculateExpenses(currentReport);
            currentReport = CalculateRevenue(currentReport);
            _BusinessReportRepository.Update(currentReport);
            _BusinessReportRepository.Save();
        }
        public void DeleteService(BusinessReportVM entity) //ONLY USED IF AN ERROR OCCURS
        {
             
            var businessReport = _FindBusinessReportRepository.FindBy(x => x.Uuid == entity.Uuid);
            _BusinessReportRepository.Delete(businessReport);
            _BusinessReportRepository.Save();
            
        }
		
		//Financial data calculations (ONLY USED WHEN REPORT IS FOUND OUTSIDE OF METHOD)
		public BusinessReport CalculateRevenue(BusinessReport entity)
        {
            //var revenue = _FindSaleRepository.ListBy(x => x.BusinessReportId == entity.Id).Sum(x => x.TotalPrice);
            var revenue = entity.Sales.Sum(x => x.TotalPrice);
            entity.Revenue = revenue;
            return entity;
        }
        public BusinessReport CalculateCost(BusinessReport entity)
        {
            //var cost = _FindExpenseRepository.ListBy(x => x.BusinessReportId == entity.Id).Sum(x => x.Cost);
            var cost = entity.Expenses.Sum(x => x.Cost);
            entity.Cost = cost;
            return entity;
        }
        public BusinessReport CalculateProfit(BusinessReport entity)
        {
            var profit = entity.Revenue - entity.Cost;
            entity.Profit = profit;
            return entity;
        }
        public BusinessReport CalculateMembershipData(BusinessReport entity)
        {
            var ClientMemberships = _FindClientRepository.ListBy(x => x.MembershipName != null);
            entity.MonthlyMembers = ClientMemberships.Where(x => x.MembershipStatus == "Active").Count();
            var MonthSales = ClientMemberships.Where(x => x.MembershipStartDate.GetValueOrDefault().Date.Month == DateTime.Now.Month);
            double TotalSales = 0;
            foreach(var item in MonthSales)
            {
                TotalSales += item.Sales.FirstOrDefault(x => x.Product.ProductType.Name == "Membership" && x.Date.Month == DateTime.Now.Month).TotalPrice;
            }
            entity.NewMembershipSales = MonthSales.Count();
            entity.MembershipFee = TotalSales;
            return entity;
        }//for the month
        public BusinessReport CalculateAccountsReceivable(BusinessReport entity)
        {
            return entity;
        } //Money owed to business by client at the end of the month
        public BusinessReport CalculateAccountsPayable(BusinessReport entity)
        {
            var clientList = _FindClientRepository.ListBy(x=>x.MembershipStatus == "Overdue");
            if(clientList == null) { return entity; }
            foreach(var client in clientList)
            {
                entity.AccountsPayable += client.Sales.FirstOrDefault(x=>x.Product.ProductType.Name == "Membership").TotalPrice;
            }
            return entity;
        }
        public BusinessReport CalculateExpenses(BusinessReport entity) //for each type of expenses
        {
            //var expenseList = _FindExpenseRepository.ListBy(x => x.BusinessReportId == entity.Id);
            var expenseList = entity.Expenses;
            var businessReportVM = new BusinessReportVM();
            foreach(var item in expenseList)
            {
                if(item.Type == "Maintenance")
                {
                    businessReportVM.Mantaince += item.Cost;
                }
                if(item.Type == "Equipment")
                {
                    businessReportVM.Equipment += item.Cost;
                }
                if(item.Type == "Utilities")
                {
                    businessReportVM.Utilities += item.Cost;
                }
                if(item.Type == "Insurance")
                {
                    businessReportVM.Insurance += item.Cost;
                }
                if(item.Type == "Other")
                {
                    businessReportVM.OtherExpenses += item.Cost;
                }
                if(item.Type == "Wage")
                {
                    businessReportVM.Wages += item.Cost;
                }
                if(item.Type == "Rent")
                {
                    businessReportVM.Rent = item.Cost;
                }
            }
            entity.Mantaince = businessReportVM.Mantaince;
            entity.Equipment = businessReportVM.Equipment;
            entity.Utilities = businessReportVM.Utilities;
            entity.Insurance = businessReportVM.Insurance;
            entity.OtherExpenses = businessReportVM.OtherExpenses;
            entity.Wages = businessReportVM.Wages;
            entity.Rent = businessReportVM.Rent;

            return entity;
        }

    }
}
