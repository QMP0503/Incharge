using Incharge.Models;
using Incharge.Service.IService;
using Incharge.ViewModels;
using Incharge.Repository.IRepository;
using AutoMapper;

namespace Incharge.Service
{
    public class ExpenseService : IService<ExpenseVM,Expense>, IRecurringExpenseService
    {
        private readonly IRepository<Expense> _ExpenseRepository;
        private readonly IFindRepository<Expense> _FindExpenseRepository;
        private readonly IFindRepository<BusinessReport> _FindBusinessReportRepository;
        private readonly IFindRepository<Employee> _FindEmployeeRepository;
        private readonly IMapper _mapper;
        private readonly IBusinessReportService _BusinessReportService;
        public ExpenseService(IFindRepository<Employee> FindEmployeeRepository, IBusinessReportService businessReportService, IFindRepository<Expense> FindExpenseRepository, IRepository<Expense> ExpenseRepository, IFindRepository<BusinessReport> FindBusinessReportRepository, IMapper mapper)
        {
            _ExpenseRepository = ExpenseRepository;
            _FindExpenseRepository = FindExpenseRepository;
            _FindBusinessReportRepository = FindBusinessReportRepository;
            _mapper = mapper;
            _BusinessReportService = businessReportService;
            _FindEmployeeRepository = FindEmployeeRepository;
        }
         
        public List<ExpenseVM> ListItem(Func<Expense, bool> predicate)
        {
            var expenseList = _FindExpenseRepository.ListBy(predicate);
            var expenseVMList = _mapper.Map<List<ExpenseVM>>(expenseList);
            return expenseVMList;
        }
        public ExpenseVM GetItem(Func<Expense, bool> predicate)
        {
            var expense = _FindExpenseRepository.FindBy(predicate);
            var expenseVM = _mapper.Map<ExpenseVM>(expense); 
            return expenseVM;   
        }
        public void AddService(ExpenseVM entity)
        {
            var expense = _mapper.Map<Expense>(entity);

            //Assigned to most recent business report (match expense date and year)
            var businessReport = _FindBusinessReportRepository.FindBy(x => x.Date.Month == entity.Date.Month && x.Date.Year == entity.Date.Year);
            expense.BusinessReport = businessReport;

            _ExpenseRepository.Add(expense);
            _ExpenseRepository.Save();
        }
        public void UpdateService(ExpenseVM entity)
        {
            var expenseToUpdate = _FindExpenseRepository.FindBy(x=>x.Uuid == entity.Uuid);
            if(expenseToUpdate == null) { throw new Exception("Expense don't exist."); }
            _mapper.Map(entity, expenseToUpdate);
            if(expenseToUpdate.Id == 0) { throw new Exception("Expense Id cannot be empty."); }
            _ExpenseRepository.Update(expenseToUpdate);
            _ExpenseRepository.Save();
        }
        public void DeleteService(ExpenseVM entity) //ONLY USED IF AN ERROR OCCURS
        {
            var expenseToDelete = _FindExpenseRepository.FindBy(x => x.Uuid == entity.Uuid);
            if(expenseToDelete == null) { throw new Exception("Expense don't exist."); }
            _ExpenseRepository.Delete(expenseToDelete);
            _ExpenseRepository.Save();
        }
        public void AddWageExpense(ExpenseVM entity)
        {
            //add checker to avoid paying someone twice
            if(entity == null) { throw new Exception("Empty entry"); }
            if(entity.EmployeeUuids == null) { throw new Exception("No Employees Selected"); }
            var businessReport = _FindBusinessReportRepository.FindBy(x => x.Date.Month == entity.Date.Month && x.Date.Year == entity.Date.Year);
            foreach (var Uuid in entity.EmployeeUuids)
            {
                var employee = _FindEmployeeRepository.FindBy(x => x.Uuid == Uuid);
                if(employee == null) { throw new Exception("Employee don't exist"); }
                if(_FindExpenseRepository.FindBy(x => x.Date.Month == entity.Date.Month && x.Name.Contains($"{employee.FirstName} {employee.LastName}") && x.Type == "Wages") == null)
                {
                    var expense = new Expense()
                    {
                        Name = $"{employee.FirstName} {employee.LastName} wage",
                        Date = entity.Date,
                        Type = "Wages",
                        Cost = Math.Round(employee.TotalSalary/12 ?? 0, 2),
                        Description = $"{employee.FirstName} {employee.LastName} wage have been tracked",
                        BusinessReport = businessReport
                    };
                    _ExpenseRepository.Add(expense);
                }
            }
            _ExpenseRepository.Save();
        }
        public void AddRecurringExpense(ExpenseVM entity)
        {
            if (entity == null) throw new Exception("Empty entry"); 
            if (entity.RecurringListUuids == null) throw new Exception("No Employees Selected");
            var businessReport = _FindBusinessReportRepository.FindBy(x => x.Date.Month == entity.Date.Month && x.Date.Year == entity.Date.Year);
            foreach(var expenseUuid in entity.RecurringListUuids) 
            {
                var expense = _FindExpenseRepository.FindBy(x => x.Uuid == expenseUuid);
                if(expense == null) { throw new Exception("Expense don't exist"); }
                if (_FindExpenseRepository.FindBy(x => x.Date.Month == entity.Date.Month && x.Name.Contains(expense.Name)) == null)
                {
                    var AddExpense = new Expense()
                    {
                        Name = $"{expense.Name}",
                        Date = entity.Date,
                        Type = expense.Type,
                        Cost = Math.Round(expense.Cost, 2),
                        Description = expense.Description,
                        BusinessReport = businessReport
                    };
                    _ExpenseRepository.Add(AddExpense);
                }
            }
            _ExpenseRepository.Save();
        }

    }
}
