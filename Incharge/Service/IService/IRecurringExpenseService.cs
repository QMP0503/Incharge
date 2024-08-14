using Incharge.ViewModels;

namespace Incharge.Service.IService
{
    public interface IRecurringExpenseService
    {
        public void AddWageExpense(ExpenseVM entity);
        public void AddRecurringExpense(ExpenseVM entity);
    }
}
