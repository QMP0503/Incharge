using Incharge.Models;
using Incharge.Service.IService;
using Incharge.ViewModels;
using Incharge.Repository.IRepository;
using AutoMapper;

namespace Incharge.Service
{
    public class ExpenseService : IService<ExpenseVM,Expense>
    {
        private readonly IRepository<Expense> _ExpenseRepository;
        private readonly IFindRepository<Expense> _FindExpenseRepository;
        private readonly IFindRepository<BusinessReport> _FindBusinessReportRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IFindRepository<Expense> FindExpenseRepository, IRepository<Expense> ExpenseRepository, IFindRepository<BusinessReport> FindBusinessReportRepository, IMapper mapper)
        {
            _ExpenseRepository = ExpenseRepository;
            _FindExpenseRepository = FindExpenseRepository;
            _FindBusinessReportRepository = FindBusinessReportRepository;
            _mapper = mapper;
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
    }
}
